using Fido2me.Data;
using Fido2me.Data.FIDO2;
using Fido2me.Models;
using Fido2me.Responses;
using Fido2NetLib;
using Fido2NetLib.Objects;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using static Fido2NetLib.Fido2;

namespace Fido2me.Services
{
    public interface IFidoRegistrationService
    {
        Task<CredentialCreateOptions> RegistrationStartAsync(string usernamey);

        Task<RegistrationResponse> RegistrationCompleteAsync(AuthenticatorAttestationRawResponse attestationResponse, CancellationToken cancellationToken);

        Task CompleteAttestation(AttestationVerificationSuccess attestationResult, long accountId);

        Task<CredentialCreateOptions> RegistrationAddNewDeviceStartAsync(string username);

        Task<RegistrationResponse> RegistrationAddNewDeviceCompleteAsync(AuthenticatorAttestationRawResponse attestationResponseJson, long accountId, CancellationToken cancellationToken);
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

        public async Task<CredentialCreateOptions> RegistrationStartAsync(string username)
        {

            var isNewUser = await _dataContext.Credentials.Where(c => c.Username == username).CountAsync();

            if (isNewUser > 0)
            {
                return new CredentialCreateOptions() { Status = "User exists.", ErrorMessage = "User exists." };
            }

            // 1. Create a temporary user to store in an encrypted cookie
            var user = new Fido2User
            {
                Name = username,
                Id = Guid.NewGuid().ToByteArray() // byte representation of userID is required
            };

            // system controlled options
            var existingKeys = new List<PublicKeyCredentialDescriptor>();
            var authenticatorSelection = new AuthenticatorSelection
            {
                RequireResidentKey = false,
                ResidentKey = ResidentKeyRequirement.Preferred,
                UserVerification = UserVerificationRequirement.Required,
            };
            var exts = new AuthenticationExtensionsClientInputs()
            {
                // Extensions = true,
                // UserVerificationMethod = true,
            };

            var options = _fido2.RequestNewCredential(user, existingKeys, authenticatorSelection, AttestationConveyancePreference.Direct, exts);
            //var registrationOptionModel = new RegistrationOptionsModel(options);
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
                    return new RegistrationResponse(RegistrationStatus.Failure) {  ErrorMessage = "Missing registration cookie" };
                }
                var unprotectedOptions = _protector.Unprotect(protectedOptions);

                var options = CredentialCreateOptions.FromJson(unprotectedOptions);
                // display name and name were not a part of initial registration
                //options.User.DisplayName = attestationResponse.DisplayName;
                //options.User.Name = attestationResponse.Name;
                _contextAccessor.HttpContext.Response.Cookies.Delete(Constants.CookieRegistration);

                // 2. Create callback so that lib can verify credential id is unique to this user
                // GUID will be unique

                // 2. Verify and make the credentials
                var attestation = await _fido2.MakeNewCredentialAsync(attestationResponse, options, null, lazyAttestation: true, cancellationToken: cancellationToken);
                
                // new registration - new account Id
                await CompleteAttestation(attestation.Result, -1);

                return new RegistrationResponse(RegistrationStatus.Success, attestation.Result);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message, e.StackTrace);
                return new RegistrationResponse(RegistrationStatus.Failure) { ErrorMessage = e.Message };

            }
        }

        public async Task CompleteAttestation(AttestationVerificationSuccess attestationResult, long accountId)
        {
            string desc = "";
            if (_mds != null)
            {
                var entry = await _mds.GetEntryAsync(attestationResult.AaGuid);
                desc = entry?.MetadataStatement?.Description;                
            }

            var account = await _dataContext.Accounts.FirstOrDefaultAsync(a => a.Id == accountId);
            if (account == null)
            {
                // new registration
                account = new Account()
                {
                    EmailVerified = false,
                    Name = attestationResult.User.Name.ToLowerInvariant().Trim(),
                    AccountType = AccountType.Personal,
                };
                await _dataContext.Accounts.AddAsync(account);
                await _dataContext.SaveChangesAsync();
            }

            var credential = new Credential()
            {
                CredentialId = attestationResult.CredentialId,
                Enabled = true,     
                Username = attestationResult.User.Name.ToLowerInvariant().Trim(),               
                AaGuid = attestationResult.AaGuid,
                DeviceDescription = desc,   
                CredType = attestationResult.CredType,
                PublicKey = attestationResult.PublicKey,
                UserHandle = attestationResult.User.Id,
                RegDate = DateTime.UtcNow,
                SignatureCounter = attestationResult.Counter,
                AccountId = account.Id,                
            };
            await _dataContext.Credentials.AddAsync(credential);
            await _dataContext.SaveChangesAsync();


            var rawAttestation = attestationResult.AttestationRawResponse;

            var attestation = new Attestation()
            {
                RawId = rawAttestation.RawId,
                AttestationObject = rawAttestation.Response.AttestationObject,
                ClientDataJson = rawAttestation.Response.ClientDataJson,
                CredentialId = credential.Id,
            };
            await _dataContext.Attestations.AddAsync(attestation);

            await _dataContext.SaveChangesAsync();

        }

        /// <summary>
        /// Prepare a challenge and other objects to add a new authenticator.
        /// </summary>
        /// <param name="username">Provide a current username from a session.</param>
        /// <returns></returns> 
        public async Task<CredentialCreateOptions> RegistrationAddNewDeviceStartAsync(string username)
        {
            // no need account?
            // var account = await _dataContext.Accounts.Where(a => a.Username == username).AsNoTracking().FirstOrDefaultAsync();
            var excludeCredentials = await _dataContext.Credentials.Where(c => c.Username == username).Select(c => new PublicKeyCredentialDescriptor(c.CredentialId)).ToListAsync();

            var user = new Fido2User
            {
                Name = username,
                Id = Guid.NewGuid().ToByteArray(), 
            };
  
            var authenticatorSelection = new AuthenticatorSelection
            {
                RequireResidentKey = false,
                ResidentKey = ResidentKeyRequirement.Preferred,
                UserVerification = UserVerificationRequirement.Required,
            };
            var exts = new AuthenticationExtensionsClientInputs()
            {
                // Extensions = true,
                // UserVerificationMethod = true,
            };

            var options = _fido2.RequestNewCredential(user, excludeCredentials, authenticatorSelection, AttestationConveyancePreference.Direct, exts);

            string protectedPayload = _protector.Protect(options.ToJson());
            _contextAccessor.HttpContext.Response.Cookies.Append(Constants.CookieAddNewDevice, protectedPayload, new CookieOptions { HttpOnly = true, IsEssential = true, Secure = true, SameSite = SameSiteMode.Strict, Expires = DateTime.Now.AddMinutes(5) });

            return await Task.FromResult(options);
        }

        public async Task<RegistrationResponse> RegistrationAddNewDeviceCompleteAsync(AuthenticatorAttestationRawResponse attestationResponse, long accountId, CancellationToken cancellationToken)
        {
            try
            {
                // 1. get the options we sent the client                
                var protectedOptions = _contextAccessor.HttpContext.Request.Cookies[Constants.CookieAddNewDevice];
                if (protectedOptions == null)
                {
                    return new RegistrationResponse(RegistrationStatus.Failure) { ErrorMessage = "Missing add new device cookie" };
                }
                var unprotectedOptions = _protector.Unprotect(protectedOptions);

                var options = CredentialCreateOptions.FromJson(unprotectedOptions);
                // display name and name were not a part of initial registration
                //options.User.DisplayName = attestationResponse.DisplayName;
                //options.User.Name = attestationResponse.Name;
                _contextAccessor.HttpContext.Response.Cookies.Delete(Constants.CookieAddNewDevice);

                // 2. Create callback so that lib can verify credential id is unique to this user
                // GUID will be unique

                // 2. Verify and make the credentials
                var attestation = await _fido2.MakeNewCredentialAsync(attestationResponse, options, null, lazyAttestation: true, cancellationToken: cancellationToken);

                // add to existing account ID
                await CompleteAttestation(attestation.Result, accountId);

                return new RegistrationResponse(RegistrationStatus.Success, attestation.Result);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message, e.StackTrace);
                return new RegistrationResponse(RegistrationStatus.Failure) { ErrorMessage = e.Message };

            }
        }
    }
}
