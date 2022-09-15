using Fido2me.Data.OIDC;
using Fido2NetLib;
using System.ComponentModel.DataAnnotations;

namespace Fido2me.Models.Applications
{
    public class OidcCreateClientViewModel
    {
        [Required]
        [MaxLength(200)]
        public string ClientId { get; set; }

        [Required]
        public OidcClientType OidcClientType { get; set; }

        [Required]
        [MaxLength(200)]
        public string ClientName { get; set; } = "";

        [MaxLength(1000)]
        public string Description { get; set; }

        [Required]
        public bool RequireClientSecret { get; set; } = true;

        public bool RequirePkce { get; set; } = true;

        //public bool? RequireConsent { get; set; }

        //public bool? AllowRememberConsent { get; set; }

        public bool? AllowOfflineAccess { get; set; }

        //public List<ClientClaim> ClientClaims { get; set; } = new List<ClientClaim>();


        [MaxLength(150)]
        public string CorsOrigin { get; set; } = "";

        [MaxLength(250)]
        public string GrantType { get; set; }

        [Required]
        [MaxLength(200)]
        public string RedirectUri { get; set; }

        //[Required]
        //[MaxLength(200)]
        //public string PostLoginUrl { get; set; }

        //[Required]
        //[MaxLength(200)]
        //public string PostLogoutUrl { get; set; }


        [MaxLength(200)]
        public string Scope { get; set; } = "openid";

        public ClientScopes ClientScopes { get; set; }

        [MaxLength(200)]
        public string ClientSecret { get; set; }


    }

    public class ClientScopes
    {
        // note: openid is always set, but no reason to send it back and forth
        public bool Email { get; set; }
        public bool Profile { get; set; }
    }
}
