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
    public interface IFidoLoginService
    {
        Task<AssertionOptions> LoginStartAsync(string username);

        Task<LoginResponse> LoginCompleteAsync(AuthenticatorAssertionRawResponse clientResponse, CancellationToken cancellationToken);

        Task<Credential> GetCredentialAsync(byte[] credentialId);

        Task UpdateCredentialAsync(Credential fidoCredential);

    }


    public class FidoLoginService : IFidoLoginService
    {
        private readonly IFido2 _fido2;
        private readonly IDataProtector _protector;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly DataContext _dataContext;
        private readonly ILogger<FidoLoginService> _logger;

        public FidoLoginService(IFido2 fido2, DataContext dataContext, IDataProtectionProvider provider, IHttpContextAccessor contextAccessor, ILogger<FidoLoginService> logger)
        {
            _fido2 = fido2;
            _dataContext = dataContext;
            _protector = provider.CreateProtector(Constants.DataProtectorName);
            _contextAccessor = contextAccessor ?? throw new ArgumentNullException(nameof(IHttpContextAccessor));
            _logger = logger;
        }

        public async Task<AssertionOptions> LoginStartAsync(string username)
        {
            var existingCredentials = new List<PublicKeyCredentialDescriptor>();
            if (string.IsNullOrWhiteSpace(username))
            {
                // usernameless flow with a resident key
            }
            else
            {
                // get a list of credentials
                var creds = await _dataContext.Credentials.Where(x => x.Username == username).Select(c => c.Id).ToListAsync();
                // TODO: rework with a proper select statement
                // Probably exclude all residential keys from the response?
                foreach (var cred in creds)
                {
                    existingCredentials.Add(new PublicKeyCredentialDescriptor(cred));
                }
            }       

        
            var exts = new AuthenticationExtensionsClientInputs()
            {
                UserVerificationMethod = true
            };

            // 3. Create options
            var options = _fido2.GetAssertionOptions(
                allowedCredentials: existingCredentials,
                UserVerificationRequirement.Required,
                exts
            );

            // 4. Temporarily store options, session/in-memory cache/redis/db
            string protectedPayload = _protector.Protect(options.ToJson());
            _contextAccessor.HttpContext.Response.Cookies.Append(Constants.CookieLogin, protectedPayload, new CookieOptions { HttpOnly = true, IsEssential = true, Secure = true, SameSite = SameSiteMode.Strict,  Expires = DateTime.Now.AddMinutes(5) });

            // 5. Return options to client
            return await Task.FromResult(options);
        }

        public async Task<LoginResponse> LoginCompleteAsync(AuthenticatorAssertionRawResponse clientResponse, CancellationToken cancellationToken)
        {
            try
            {
                // Get the assertion options we sent the client
                var protectedOptions = _contextAccessor.HttpContext.Request.Cookies[Constants.CookieLogin];
                if (protectedOptions == null)
                {
                    return new LoginResponse(LoginResponseStatus.Error) { ErrorMessage = "Missing login cookie." };
                }
                var unprotectedOptions = _protector.Unprotect(protectedOptions);

                var options = AssertionOptions.FromJson(unprotectedOptions);
                _contextAccessor.HttpContext.Response.Cookies.Delete(Constants.CookieLogin);

                // Get registered credential from database
                var credential = await GetCredentialAsync(clientResponse.Id);
                // Check Raw.Id == UserHandle? (check if userhandle owns the credentialId)
                if (credential == null)
                {
                    return new LoginResponse(LoginResponseStatus.Error) { ErrorMessage = "Credential not found." };
                }

                // Get credential counter from database
                var storedCounter = credential.SignatureCounter;  

                // Make the assertion
                var res = await _fido2.MakeAssertionAsync(clientResponse, options, credential.PublicKey, storedCounter, null, cancellationToken: cancellationToken);
                
                // Store the updated counter
                credential.SignatureCounter = res.Counter;
                await UpdateCredentialAsync(credential);
                
                var loginResponseStatus = res.Status == "ok" ? LoginResponseStatus.Success : LoginResponseStatus.Error;

                var r = new LoginResponse(loginResponseStatus)
                {
                    AaGuid = credential.AaGuid,
                    AccountId = credential.AccountId,
                    CredentialId = credential.Id.ToString(),
                    DeviceName = credential.Username,
                    DeviceDisplayName = credential.DeviceDisplayName,               
                    ErrorMessage = res.ErrorMessage,                    
                };
                
                /*
                var user = new IdentityServerUser(credential.AccountId.ToString())
                {
                    DisplayName = credential.AaGuid.ToString()
                };
                await HttpContext.SignInAsync(user);
                */

                return r;
            }
            catch (Exception e)
            {
                return new LoginResponse(LoginResponseStatus.Error) { ErrorMessage = e.Message }; 
            }
        }

 

        public async Task<Credential> GetCredentialAsync(byte[] credentialId)
        {
            var credential = await _dataContext.Credentials.FirstOrDefaultAsync(c => c.Id == credentialId && c.Enabled);
            return credential;
        }

        public async Task UpdateCredentialAsync(Credential fidoCredential)
        {
            _dataContext.Credentials.Update(fidoCredential);
            await _dataContext.SaveChangesAsync();

        }

    }
}
