using Duende.IdentityServer;
using Duende.IdentityServer.Models;

namespace Fido2me.Duende.Test
{
    public class TestClients
    {
        public static IEnumerable<Client> Get()
        {
            return new List<Client>
            {
                new Client
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
                    }
                },            
            };
        }
    }
}
