using Duende.IdentityServer;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Stores;
using Fido2me.Data;
using Microsoft.EntityFrameworkCore;

namespace Fido2me.Duende
{
    public class ClientStore : IClientStore
    {
        private readonly DataContext _dataContext;
        private readonly IConfiguration _configuration;

        public ClientStore(DataContext dataContext, IConfiguration configuration)
        {
            _dataContext = dataContext;
            _configuration = configuration;
        }
        public async Task<Client> FindClientByIdAsync(string clientId)
        {
            // fail fast and do not consume resources and db calls            
            if (!Guid.TryParse(clientId, out var clientGuid))
            {
                return null;
            }

            // first party CIBA client, don't do a lookup
            if (clientId == _configuration[Constants.ConfigCibaClientId])
            {
                return new Client
                {
                    ClientId = _configuration[Constants.ConfigCibaClientId],
                    ClientName = "ciba",
                    ClientSecrets = { new Secret(_configuration[Constants.ConfigCibaClientSecret].Sha256()) },
                    AllowedGrantTypes = GrantTypes.Ciba,
                    RequireConsent = true,
                    AllowOfflineAccess = false,
                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                    },
                };
            }

            // get client from db
            var oidcBasicClient = await _dataContext.OidcBasicClients.FirstOrDefaultAsync(c => c.ClientId == clientId && c.Enabled == true);
            if (oidcBasicClient == null)
            {
                return null;
            }

            var client = oidcBasicClient?.ToIdentityModel();
            
            return client;
        }
    }
}
