using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fido2me.Data.OIDC
{
    /// <summary>
    /// Simplified version of OIDC Client that will use DuendeModel defaults.
    /// The goal is to provide Confidential and Public client features only.
    /// </summary>
    public class OidcBasicClient
    {
        [Key]
        [Required]
        // System generated GUID in lower case hex format
        public string ClientId { get; set; }

        [Required]
        public Guid AccountId { get; set; }

        [Required]
        public bool Enabled { get; set; }

        [Required]
        public bool RequireClientSecret { get; set; }

        [MaxLength(200)]
        public string ClientName { get; set; }

        [MaxLength(1000)]
        public string Description { get; set; }

        [MaxLength(2000)]
        public string ClientUri { get; set; }

        [MaxLength(2000)]
        public string LogoUri { get; set; }

        [Required]
        public bool RequireConsent { get; set; }

        [Required]
        public bool AllowRememberConsent { get; set; }

        [Required]
        public bool AllowOfflineAccess { get; set; }

        [Required]
        public bool RequirePkce { get; set; }

        [Required]
        public DateTimeOffset Created { get; set; }

        public DateTimeOffset? Updated { get; set; }
       
        public List<ClientClaim> ClientClaims { get; set; } = new List<ClientClaim>();

        public string[] ClientCorsOrigins { get; set; }

        public string[] ClientGrantTypes { get; set; }

        public string[] ClientRedirectUris { get; set; }

        public string[] ClientScopes { get; set; }

        public List<ClientSecret> ClientSecrets { get; set; } = new List<ClientSecret>();
        

        public OidcBasicClient() { }

    }
}
