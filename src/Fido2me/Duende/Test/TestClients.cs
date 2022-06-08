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
                ClientId = "oauthClient",
                ClientName = "Example client application using client credentials",
                AllowedGrantTypes = GrantTypes.ClientCredentials,
                ClientSecrets = new List<Secret> {new Secret("SuperSecretPassword".Sha256())}, // change me!
                AllowedScopes = new List<string> {"api1.read"}
            }
        };
        }
    }
}
