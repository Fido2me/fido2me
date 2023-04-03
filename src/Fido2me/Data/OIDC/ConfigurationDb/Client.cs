using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Fido2me.Data.OIDC.ConfigurationDb
{
    [Table("Clients")]
    [Index(nameof(ClientId), IsUnique = true)]
    public class Client
    {      
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Required]
        public long AccountId { get; set; }

        [Required]
        public bool Enabled { get; set; }

        [Required]
        [StringLength(200)]
        public string ClientId { get; set; }

        [Required]
        [StringLength(200)]
        public string ProtocolType { get; set; }

        [Required]
        public bool RequireClientSecret { get; set; }

        [StringLength(2000)]
        public string ClientName { get; set; }

        [StringLength(1000)]
        public string Description { get; set; }

        [StringLength(2000)]
        public string ClientUri { get; set; }

        [StringLength(2000)]
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

        [StringLength(2000)]
        public string FrontChannelLogoutUri { get; set; }

        [Required]
        public bool FrontChannelLogoutSessionRequired { get; set; }

        [StringLength(2000)]
        public string BackChannelLogoutUri { get; set; }

        [Required]
        public bool BackChannelLogoutSessionRequired { get; set; }

        [Required]
        public bool AllowOfflineAccess { get; set; }

        [Required]
        public int IdentityTokenLifetime { get; set; }

        [StringLength(100)]
        public string AllowedIdentityTokenSigningAlgorithms { get; set; }

        [Required]
        public int AccessTokenLifetime { get; set; }

        [Required]
        public int AuthorizationCodeLifetime { get; set; }

        public int? ConsentLifetime { get; set; } = null;

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

        [StringLength(200)]
        public string ClientClaimsPrefix { get; set; }

        [StringLength(200)]
        public string PairWiseSubjectSalt { get; set; }

        public int? UserSsoLifetime { get; set; } = null;

        [StringLength(100)]
        public string UserCodeType { get; set; }

        [Required]
        public int DeviceCodeLifetime { get; set; }

        public int? CibaLifetime { get; set; } = null;

        public int? PollingInterval { get; set; } = null;

        public bool? CoordinateLifetimeWithUserSession { get; set; } = null;

        [Required]
        public DateTime Created { get; set; } = DateTime.UtcNow;

        public DateTime Updated { get; set; }

        public DateTime LastAccessed { get; set; }

        [Required]
        public bool NonEditable { get; set; }

        public IList<ClientSecret> ClientSecrets { get; set; } = new List<ClientSecret>();

        public IList<ClientRedirectUri> ClientRedirectUris { get; set; } = new List<ClientRedirectUri>();
    }
}
