using Fido2me.Data;
using Fido2me.Data.FIDO2;
using Fido2me.Models;
using Fido2me.Responses;
using Fido2NetLib;
using Fido2NetLib.Objects;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.Logging;
using static Fido2NetLib.Fido2;

namespace Fido2me.Services
{
    public interface IFidoRegistrationService
    {
        Task<CredentialCreateOptions> RegistrationStartAsync();

        Task<RegistrationResponse> RegistrationCompleteAsync(AuthenticatorAttestationRawResponse attestationResponse, CancellationToken cancellationToken);

        Task CompleteAttestation(BaseAttestationVerification attestationResult, Guid accountId);
    }


    public class FidoRegistrationService : IFidoRegistrationService
    {
        private readonly IFido2 _fido2;
        public static IMetadataService _mds;
        private readonly IDataProtector _protector;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly DataContext _dataContext;
        private readonly ILogger<FidoRegistrationService> _logger;

        public FidoRegistrationService(IFido2 fido2, IMetadataService mds, DataContext dataContext, IDataProtectionProvider provider, IHttpContextAccessor contextAccessor, ILogger<FidoRegistrationService> logger)
        {
            _fido2 = fido2;
            _mds = mds;
            _dataContext = dataContext;
            _protector = provider.CreateProtector(Constants.DataProtectorName);
            _contextAccessor = contextAccessor ?? throw new ArgumentNullException(nameof(IHttpContextAccessor));
            _logger = logger;
        }

        public async Task<CredentialCreateOptions> RegistrationStartAsync()
        {
            // we will allow to change it later
            var displayName = $"({DateTimeOffset.Now})";

            // 1. Create a temporary user to store in an encrypted cookie
            var user = new Fido2User
            {
                DisplayName = displayName,
                Name = displayName,
                Id = Guid.NewGuid().ToByteArray() // byte representation of userID is required
            };

            // 2. Get user existing keys by username            
            // it's a new registration, we will try to add more credentials or merge accounts later, RP will allow to create multiple accounts on the same single authenticator
            var existingKeys = new List<PublicKeyCredentialDescriptor>();

            // 3. Create options
            var authenticatorSelection = new AuthenticatorSelection
            {
                RequireResidentKey = true,
                UserVerification = UserVerificationRequirement.Required,
            };

            // accepting any authenticator attachment    
            var exts = new AuthenticationExtensionsClientInputs()
            {
                Extensions = true,
                UserVerificationMethod = true,
            };

            var options = _fido2.RequestNewCredential(user, existingKeys, authenticatorSelection, AttestationConveyancePreference.Direct, exts);
            var registrationOptionModel = new RegistrationOptionsModel(options);
            string protectedPayload = _protector.Protect(options.ToJson());
            _contextAccessor.HttpContext.Response.Cookies.Append(Constants.CookieRegistration, protectedPayload, new CookieOptions { HttpOnly = true, IsEssential = true, Secure = true, SameSite = SameSiteMode.Strict, Expires = DateTime.Now.AddMinutes(5) });

            return await Task.FromResult(options);
        }

        public async Task<RegistrationResponse> RegistrationCompleteAsync(AuthenticatorAttestationRawResponse attestationResponse, CancellationToken cancellationToken)
        {
            try
            {
                // 1. get the options we sent the client                
                var protectedOptions = _contextAccessor.HttpContext.Request.Cookies[Constants.CookieRegistration];
                if (protectedOptions == null)
                {
                    return new RegistrationResponse(AttestationVerificationStatus.Failed) {  ErrorMessage = "Missing registration cookie" };
                }
                var unprotectedOptions = _protector.Unprotect(protectedOptions);

                var options = CredentialCreateOptions.FromJson(unprotectedOptions);
                // display name and name were not a part of initial registration
                options.User.DisplayName = attestationResponse.DisplayName;
                options.User.Name = attestationResponse.Name;
                _contextAccessor.HttpContext.Response.Cookies.Delete(Constants.CookieRegistration);

                // 2. Create callback so that lib can verify credential id is unique to this user
                // GUID will be unique

                // 2. Verify and make the credentials
                var attestation = await _fido2.MakeNewCredentialAsync(attestationResponse, options, null, lazyAttestation: true, cancellationToken: cancellationToken);
                if (attestation.Result.AttestationVerificationStatus == AttestationVerificationStatus.Failed)
                { }// bad request

                // new registration - new account Id
                await CompleteAttestation(attestation.Result, Guid.NewGuid());

                return new RegistrationResponse(attestation.Result.AttestationVerificationStatus);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message, e.StackTrace);
                return new RegistrationResponse(AttestationVerificationStatus.Failed) { ErrorMessage = e.Message };

            }
        }

        public async Task CompleteAttestation(BaseAttestationVerification attestationResult, Guid accountId)
        {
            string desc = "";
            if (_mds != null)
            {
                var entry = await _mds.GetEntryAsync(attestationResult.Aaguid);
                desc = entry?.MetadataStatement?.Description;                
            }

            var credential = new Credential()
            {
                Id = attestationResult.CredentialId,
                Enabled = true,
                CredentialId = Convert.ToHexString(attestationResult.CredentialId),
                DeviceName = attestationResult.User.Name,
                DeviceDisplayName = attestationResult.User.DisplayName,
                AaGuid = attestationResult.Aaguid,
                DeviceDescription = desc,   
                CredType = attestationResult.CredType,
                PublicKey = attestationResult.PublicKey,
                UserHandle = attestationResult.User.Id,
                RegDate = DateTimeOffset.Now,
                SignatureCounter = attestationResult.Counter,
                AccountId = accountId,
                AttestionResult = attestationResult.AttestationVerificationStatus.ToString()
            };
            await _dataContext.Credentials.AddAsync(credential);

            if (attestationResult.AttestationVerificationStatus == AttestationVerificationStatus.SuccessNoAttestation && attestationResult is LazyAttestationVerificationSuccess)
            {
                var rawAttestation = (attestationResult as LazyAttestationVerificationSuccess).AttestationRawResponse;

                var attestation = new Attestation()
                {
                    Id = rawAttestation.Id,
                    RawId = rawAttestation.RawId,
                    AttestationObject = rawAttestation.Response.AttestationObject,
                    ClientDataJson = rawAttestation.Response.ClientDataJson

                };
                await _dataContext.Attestations.AddAsync(attestation);
            }

            var account = new Account()
            {
                Id = credential.AccountId,
                EmailVerified = false,                
            };
            
            await _dataContext.Accounts.AddAsync(account);

            await _dataContext.SaveChangesAsync();

        }
    }
}
