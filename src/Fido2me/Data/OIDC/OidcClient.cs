using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fido2me.Data.OIDC
{
    public class OidcClient
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        public bool Enabled { get; set; }

        [Required]
        [MaxLength(200)]
        public string ClientId { get; set; }

        [Required]
        [MaxLength(200)]
        public string ProtocolType { get; set; }

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
        public bool AlwaysIncludeUserClaimsInIdToken { get; set; }

        [Required]
        public bool RequirePkce { get; set; }

        [Required]
        public bool AllowPlainTextPkce { get; set; }

        [Required]
        public bool RequireRequestObject { get; set; }

        [Required]
        public bool AllowAccessTokensViaBrowser { get; set; }

        [MaxLength(2000)]
        public string FrontChannelLogoutUri { get; set; }

        [Required]
        public bool FrontChannelLogoutSessionRequired { get; set; }

        [MaxLength(2000)]
        public string BackChannelLogoutUri { get; set; }

        [Required]
        public bool BackChannelLogoutSessionRequired { get; set; }

        [Required]
        public bool AllowOfflineAccess { get; set; }

        [Required]
        public int IdentityTokenLifetime { get; set; }

        [MaxLength(1000)]
        public string AllowedIdentityTokenSigningAlgorithms { get; set; }

        [Required]
        public int AccessTokenLifetime { get; set; }

        [Required]
        public int AuthorizationCodeLifetime { get; set; }

        public int? ConsentLifetime { get; set; }

        [Required]
        public int AbsoluteRefreshTokenLifetime { get; set; }

        [Required]
        public int SlidingRefreshTokenLifetime { get; set; }

        [Required]
        public int RefreshTokenUsage { get; set; }

        [Required]
        public bool UpdateAccessTokenClaimsOnRefresh { get; set; }

        [Required]
        public int RefreshTokenExpiration { get; set; }

        [Required]
        public int AccessTokenType { get; set; }

        [Required]
        public bool EnableLocalLogin { get; set; }

        [Required]
        public bool IncludeJwtId { get; set; }

        [Required]
        public bool AlwaysSendClientClaims { get; set; }

        [MaxLength(200)]
        public string ClientClaimsPrefix { get; set; }

        [MaxLength(200)]
        public string PairWiseSubjectSalt { get; set; }

        public int? UserSsoLifetime { get; set; }

        [MaxLength(100)]
        public string UserCodeType { get; set; }

        [Required]
        public int DeviceCodeLifetime { get; set; }

        public int? CibaLifetime { get; set; }

        public int? PollingInterval { get; set; }

        public bool? CoordinateLifetimeWithUserSession { get; set; }

        [Required]
        public DateTime Created { get; set; }

        public DateTime? Updated { get; set; }

        public DateTime? LastAccessed { get; set; }

        [Required]
        public bool NonEditable { get; set; }

        public List<ClientClaim> ClientClaims { get; set; } = new List<ClientClaim>();

        public List<ClientCorsOrigin> ClientCorsOrigins { get; set; } = new List<ClientCorsOrigin>();

        public List<ClientGrantType> ClientGrantTypes { get; set; } = new List<ClientGrantType>();

        public List<ClientIdPRestriction> ClientIdPRestrictions { get; set; } = new List<ClientIdPRestriction>();

        public List<ClientPostLogoutRedirectUri> ClientPostLogoutRedirectUris { get; set; } = new List<ClientPostLogoutRedirectUri>();

        public List<ClientProperty> ClientProperties { get; set; } = new List<ClientProperty>();

        public List<ClientRedirectUri> ClientRedirectUris { get; set; } = new List<ClientRedirectUri>();

        public List<ClientScope> ClientScopes { get; set; } = new List<ClientScope>();

        public List<ClientSecret> ClientSecrets { get; set; } = new List<ClientSecret>();
        

        public OidcClient() { }
    }
}
