using IdentityModel.Client;

namespace Fido2me.Services
{
    public interface IDiscoveryService
    {
        Task<string> GetCibaEndpointAsync();
    }

    public class DiscoveryService : IDiscoveryService
    {
        private static IDiscoveryCache _cache;
        private readonly ILogger<DiscoveryService> _logger;

        public DiscoveryService(ILogger<DiscoveryService> logger, IConfiguration configuration)
        {
            _logger = logger;
            var discoEndpoint = configuration["discoEndpoint"] ?? throw new ArgumentNullException(nameof(IConfiguration)); ;
            
            _cache = new DiscoveryCache(discoEndpoint);
        }

        public async Task<string> GetCibaEndpointAsync()
        {
            var disco = await _cache.GetAsync();
            if (disco.IsError) 
                throw new Exception(disco.Error);

            return disco.BackchannelAuthenticationEndpoint;
        }
    }
}
