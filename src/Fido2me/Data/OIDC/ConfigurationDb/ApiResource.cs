using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fido2me.Data.OIDC.ConfigurationDb
{
    [Table("ApiResources")]
    public class ApiResource
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public bool Enabled { get; set; }

        [Required]
        [StringLength(200)]
        public string Name { get; set; }

        [StringLength(200)]
        public string DisplayName { get; set; }

        [StringLength(1000)]
        public string Description { get; set; }

        [StringLength(100)]
        public string AllowedAccessTokenSigningAlgorithms { get; set; }

        [Required]
        public bool ShowInDiscoveryDocument { get; set; }

        [Required]
        public bool RequireResourceIndicator { get; set; }

        [Required]
        public DateTime Created { get; set; }

        public DateTime? Updated { get; set; } = null;

        public DateTime? LastAccessed { get; set; } = null;

        [Required]
        public bool NotEditable { get; set; }
    }
}
