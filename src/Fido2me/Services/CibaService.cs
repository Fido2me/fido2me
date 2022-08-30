using Fido2me.Pages.auth;
using IdentityModel;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.DataProtection;
using System.Text.Json;
using BackchannelAuthenticationResponse = IdentityModel.Client.BackchannelAuthenticationResponse;

namespace Fido2me.Services
{
    public interface ICibaService
    {
        Task<BackchannelAuthenticationResponse> CibaLoginStartAsync(string username, string bindingMessage);
        CibaLoginViewModel GetAuthenticationRequestDetails();
        Task<TokenResponse> CibaTryLoginCompleteAsync(string authRequestId);
        //BackchannelAuthenticationResponse
    }

    public class CibaService : ICibaService
    {
        private readonly IDataProtector _protector;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IDiscoveryService _discoveryService;
        private readonly ILogger<CibaService> _logger;
        private readonly ISystemClock _systemClock;

        public CibaService(IDiscoveryService discoveryService, ILogger<CibaService> logger, IDataProtectionProvider provider, IHttpContextAccessor contextAccessor, ISystemClock systemClock)
        {
            _protector = provider.CreateProtector(Constants.DataProtectorName);
            _contextAccessor = contextAccessor;
            _discoveryService = discoveryService;
            _systemClock = systemClock;
        }
        public async Task<BackchannelAuthenticationResponse> CibaLoginStartAsync(string username, string bindingMessage)
        {
            var cibaEndpoint = await _discoveryService.GetCibaEndpointAsync();            

            var req = new BackchannelAuthenticationRequest()
            {
                Address = cibaEndpoint,
                ClientId = "ciba",
                ClientSecret = "secret",
                Scope = "openid profile offline_access",
                LoginHint = username,
                BindingMessage = bindingMessage,
                RequestedExpiry = 200
            };

            var client = new HttpClient();
            var response = await client.RequestBackchannelAuthenticationAsync(req);

            if (!response.IsError)
            {
                var cibaVm = new CibaLoginViewModel() 
                {
                    Message = bindingMessage,
                    Username = username,
                    RequestId = response.AuthenticationRequestId,
                }; 
                string protectedRequestId = _protector.Protect(JsonSerializer.Serialize(cibaVm));
                _contextAccessor.HttpContext.Response.Cookies.Append(Constants.CookieCibaRequest, protectedRequestId, new CookieOptions { HttpOnly = true, IsEssential = true, Secure = true, SameSite = SameSiteMode.Strict, Expires = DateTime.Now.AddSeconds(response.ExpiresIn) });
            }

            return response;
        }

        public CibaLoginViewModel GetAuthenticationRequestDetails()
        {
            var protectedResponse = _contextAccessor.HttpContext.Request.Cookies[Constants.CookieCibaRequest];
            if (protectedResponse == null)
            {
                return null;
            }
            var unprotectedResponse = _protector.Unprotect(protectedResponse);
            var cibaVm = JsonSerializer.Deserialize<CibaLoginViewModel>(unprotectedResponse);
            

            return cibaVm;
            // delete on success only?
            // _contextAccessor.HttpContext.Response.Cookies.Delete(Constants.CookieRegistration);
        }


        public async Task<TokenResponse> CibaTryLoginCompleteAsync(string authRequestId)
        {
            var tokenEndpoint = await _discoveryService.GetTokenEndpointAsync();

            var client = new HttpClient();
            var t = _systemClock.UtcNow.UtcDateTime;

            // https://github.com/DuendeSoftware/IdentityServer/blob/50e71c72eb3905acc9fda4e0fa86c2510db665ea/src/IdentityServer/Validation/Default/BackchannelAuthenticationRequestIdValidator.cs
            var response = await client.RequestBackchannelAuthenticationTokenAsync(new BackchannelAuthenticationTokenRequest
            {
                Address = tokenEndpoint,
                ClientId = "ciba",
                ClientSecret = "secret",
                AuthenticationRequestId = authRequestId,
            });

            
            return response;
        }
        
    }
}
