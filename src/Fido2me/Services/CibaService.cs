using IdentityModel.Client;
using Microsoft.AspNetCore.DataProtection;
using BackchannelAuthenticationResponse = IdentityModel.Client.BackchannelAuthenticationResponse;

namespace Fido2me.Services
{
    public interface ICibaService
    {
        Task<BackchannelAuthenticationResponse> CibaLoginStartAsync(string username);
        //BackchannelAuthenticationResponse
    }

    public class CibaService : ICibaService
    {
        private readonly IDataProtector _protector;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IDiscoveryService _discoveryService;
        private readonly ILogger<CibaService> _logger;

        public CibaService(IDiscoveryService discoveryService, ILogger<CibaService> logger, IDataProtectionProvider provider, IHttpContextAccessor contextAccessor)
        {
            _protector = provider.CreateProtector(Constants.DataProtectorName);
            _contextAccessor = contextAccessor;
            _discoveryService = discoveryService;
        }

        public async Task<BackchannelAuthenticationResponse> CibaLoginStartAsync(string username)
        {
            var cibaEndpoint = await _discoveryService.GetCibaEndpointAsync();

            var bindingMessage = Guid.NewGuid().ToString("N").Substring(0, 10);

            var req = new BackchannelAuthenticationRequest()
            {
                Address = cibaEndpoint,
                ClientId = "ciba",
                ClientSecret = "secret",
                Scope = "openid profile offline_access",
                LoginHint = username,
                //IdTokenHint = "eyJhbGciOiJSUzI1NiIsImtpZCI6IkYyNjZCQzA3NTFBNjIyNDkzMzFDMzI4QUQ1RkIwMkJGIiwidHlwIjoiSldUIn0.eyJpc3MiOiJodHRwczovL2xvY2FsaG9zdDo1MDAxIiwibmJmIjoxNjM4NDc3MDE2LCJpYXQiOjE2Mzg0NzcwMTYsImV4cCI6MTYzODQ3NzMxNiwiYXVkIjoiY2liYSIsImFtciI6WyJwd2QiXSwiYXRfaGFzaCI6ImE1angwelVQZ2twczBVS1J5VjBUWmciLCJzaWQiOiIzQTJDQTJDNjdBNTAwQ0I2REY1QzEyRUZDMzlCQTI2MiIsInN1YiI6IjgxODcyNyIsImF1dGhfdGltZSI6MTYzODQ3NzAwOCwiaWRwIjoibG9jYWwifQ.GAIHXYgEtXw5NasR0zPMW3jSKBuWujzwwnXJnfHdulKX-I3r47N0iqHm5v5V0xfLYdrmntjLgmdm0DSvdXswtZ1dh96DqS1zVm6yQ2V0zsA2u8uOt1RG8qtjd5z4Gb_wTvks4rbUiwi008FOZfRuqbMJJDSscy_YdEJqyQahdzkcUnWZwdbY8L2RUTxlAAWQxktpIbaFnxfr8PFQpyTcyQyw0b7xmYd9ogR7JyOff7IJIHPDur0wbRdpI1FDE_VVCgoze8GVAbVxXPtj4CtWHAv07MJxa9SdA_N-lBcrZ3PHTKQ5t1gFXwdQvp3togUJl33mJSru3lqfK36pn8y8ow",
                BindingMessage = bindingMessage,
                RequestedExpiry = 200
            };

            var client = new HttpClient();
            var response = await client.RequestBackchannelAuthenticationAsync(req);

            return response;
        }

        public async Task Do()
        {
            //string protectedPayload = _protector.Protect(options.ToJson());
            //_contextAccessor.HttpContext.Response.Cookies.Append(Constants.CookieRegistration, protectedPayload, new CookieOptions { HttpOnly = true, IsEssential = true, Secure = true, SameSite = SameSiteMode.Strict, Expires = DateTime.Now.AddMinutes(5) });
        }
    }
}
