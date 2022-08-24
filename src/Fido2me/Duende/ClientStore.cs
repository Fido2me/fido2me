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
        public ClientStore(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
        public async Task<Client> FindClientByIdAsync(string clientId)
        {
            return new Client
            {
                ClientId = "ciba",
                ClientName = "ciba",
                ClientSecrets = { new Secret("secret".Sha256()) },
                AllowedGrantTypes = GrantTypes.Ciba,
                RequireConsent = true,
                AllowOfflineAccess = true,
                AllowedScopes =
                {
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile,
                },
            };
                


            var validGuid = Guid.TryParse(clientId, out var clientGuid);
            if (!validGuid)
            {
                return null;
            }

            var oidcBasicClient = await _dataContext.OidcBasicClients.FirstOrDefaultAsync(c => c.Id == clientGuid);
            if (oidcBasicClient == null)
            {
                // how it handles null?
            }

            var client = oidcBasicClient?.ToIdentityModel();
            // Task.FromResult(client);
            return client;
        }
    }
}
