using IdentityModel.Client;

namespace Fido2me.Services
{
    public interface IDiscoveryService
    {
        Task<string> GetCibaEndpointAsync();

        Task<string> GetTokenEndpointAsync();
    }

    public class DiscoveryService : IDiscoveryService
    {
        private static IDiscoveryCache _cache;
        private readonly ILogger<DiscoveryService> _logger;

        public DiscoveryService(ILogger<DiscoveryService> logger, IConfiguration configuration)
        {
            _logger = logger;
            var discoEndpoint = configuration["oidc:discoEndpoint"] ?? throw new ArgumentNullException(nameof(IConfiguration));

            // oidc:discoEndpoint is localhost, not fido2me.com
            // have to use localhost for now to reach .well-known/openid-configuration endpoint from a container
            var discoveryPolicy = new DiscoveryPolicy()
            { 
                ValidateIssuerName = false,
                ValidateEndpoints = false,
            };
            _cache = new DiscoveryCache(discoEndpoint, discoveryPolicy);            
        }

        public async Task<string> GetCibaEndpointAsync()
        {
            var disco = await _cache.GetAsync();
            if (disco.IsError) 
                throw new Exception(disco.Error);

            return disco.BackchannelAuthenticationEndpoint;
        }

        public async Task<string> GetTokenEndpointAsync()
        {
            var disco = await _cache.GetAsync();
            if (disco.IsError)
                throw new Exception(disco.Error);

            return disco.TokenEndpoint;
        }
    }
}
