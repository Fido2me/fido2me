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
            if (clientId == "ciba")
            {
                return new Client
                {
                    ClientId = "ciba",
                    ClientName = "ciba",
                    ClientSecrets = { new Secret("secret".Sha256()) },
                    AllowedGrantTypes = GrantTypes.Ciba,
                    RequireConsent = true,
                    AllowOfflineAccess = false,
                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                    },
                };
            }
            else if (clientId == "interactive")
            {
                return new Client
                {
                    ClientId = "interactive",
                    ClientSecrets = { new Secret("secret".Sha256()) },

                    AllowedGrantTypes = GrantTypes.Code,

                    RedirectUris = { "https://localhost:44300/signin-oidc" },
                    FrontChannelLogoutUri = "https://localhost:44300/signout-oidc",
                    PostLogoutRedirectUris = { "https://localhost:44300/signout-callback-oidc" },

                    AllowOfflineAccess = true,
                    AllowedScopes = { "openid", "profile" }
                };
            }


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
