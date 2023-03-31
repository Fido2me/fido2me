using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fido2me.Data.OIDC.ConfigurationDb
{
    [Table("ApiScopes")]
    [Index(nameof(Name), IsUnique = true)]
    public class ApiScope
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Required]
        public bool Enabled { get; set; }

        [Required]
        [StringLength(200)]
        public string Name { get; set; }

        [StringLength(200)] 
        public string DisplayName { get; set; }

        [StringLength(1000)]
        public string Description { get; set; }

        [Required]
        public bool Required { get; set; }

        [Required]
        public bool Emphasize { get; set; }

        [Required]
        public bool ShowInDiscoveryDocument { get; set; }

        [Required]
        public DateTime Created { get; set; } = DateTime.UtcNow;

        public DateTime Updated { get; set; }

        public DateTime LastAccessed { get; set; }

        [Required]
        public bool NonEditable { get; set; }
    }
}
