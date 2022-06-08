using Duende.IdentityServer.Models;
using Fido2me.Data.OIDC;

namespace Fido2me.Data
{
    public static class OidcExtenstions
    {
        public static Client ToIdentityModel(this OidcBasicClient oidcc)
        {
            var ic = new Client()
            {
                ClientId = oidcc.ClientId,
                ClientName = oidcc.ClientName,
                RequireClientSecret = oidcc.RequireClientSecret,
                AllowedGrantTypes = GrantTypes.Code,
                ClientSecrets = oidcc.ClientSecrets.Select(c => new Secret { Type = c.Type, Value = c.Value, Description = c.Description, Expiration = c.Expiration }).ToList(), //new List<Secret> { new Secret("6EAAB749-D3A4-4C9C-B58F-EFFEC357FDBD") }, 
                AllowedScopes = oidcc.ClientScopes,
                AllowedCorsOrigins = oidcc.ClientCorsOrigins,
            };
            return ic;
        }

        public static OidcBasicClient FromIdentityModel(this Client ic)
        {
            var created = DateTime.Now;  // TODO: Created should not be hardcoded this way
            var oidc = new OidcBasicClient()
            {
                AllowOfflineAccess = ic.AllowOfflineAccess,
                AllowRememberConsent = ic.AllowRememberConsent,
                ClientClaims = ic.Claims.Select(c => new OIDC.ClientClaim { Type = c.Type, Value = c.Value }).ToList(),
                ClientCorsOrigins = ic.AllowedCorsOrigins.ToArray(),
                ClientGrantTypes = ic.AllowedGrantTypes.ToArray(),
                ClientId = ic.ClientId,
                ClientName = ic.ClientName,
                ClientRedirectUris = ic.RedirectUris.ToArray(),
                ClientScopes = ic.AllowedScopes.ToArray(),
                ClientSecrets = ic.ClientSecrets.Select(s => new ClientSecret { Type = s.Type, Value = s.Value, Expiration = s.Expiration, Description = s.Description, Created = created }).ToList(), //TODO  
                ClientUri = ic.ClientUri,
                Created = created, //TODO
                Description = ic.Description,
                Enabled = ic.Enabled,
                RequireClientSecret = ic.RequireClientSecret,
                RequireConsent = ic.RequireConsent,
                Updated = created, //TODO
                LogoUri = ic.LogoUri,
            };
            return oidc;
        }
    }
}
