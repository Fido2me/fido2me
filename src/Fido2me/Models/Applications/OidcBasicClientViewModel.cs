using Fido2me.Data.OIDC;
using Fido2NetLib;
using System.ComponentModel.DataAnnotations;

namespace Fido2me.Models.Applications
{
    public class OidcBasicClientViewModel
    {
        public bool? Enabled { get; set; }

        public string ClientId { get; set; }

        [Required]
        public string ClientType { get; set; } = "Public";

        [Required]
        public bool RequireClientSecret { get; set; }

        [Required]
        [MaxLength(200)]
        public string ClientName { get; set; } = "";

        [MaxLength(1000)]
        public string Description { get; set; }

        // public bool? RequireConsent { get; set; }

        //public bool? AllowRememberConsent { get; set; }

        public bool? AllowOfflineAccess { get; set; }

        public DateTime? Created { get; set; }

        public DateTime? Updated { get; set; }

        //public List<ClientClaim> ClientClaims { get; set; } = new List<ClientClaim>();


        [MaxLength(150)]
        public string CorsOrigin { get; set; } = "";

        [MaxLength(250)]
        public string GrantType { get; set; }

        [Required]
        [MaxLength(200)]
        public string RedirectUri { get; set; }

        [Required]
        [MaxLength(200)]
        public string PostLoginUrl { get; set; }

        [Required]
        [MaxLength(200)]
        public string PostLogoutUrl { get; set; }


        [MaxLength(200)]
        public string Scope { get; set; } = "openid profile email";

        [MaxLength(200)]
        public string ClientSecret { get; set; }


    }
}
