using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fido2me.Data.OIDC.ConfigurationDb
{
    [Table("IdentityProviders")]
    [Index(nameof(Scheme), IsUnique = true)]
    public class IdentityProvider
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Scheme { get; set; }

        [StringLength(200)]
        public string DisplayName { get; set; }

        [Required]
        public bool Enabled { get; set; }

        [Required]
        [StringLength(20)]
        public string Type { get; set; }

        public string Properties { get; set; }

        [Required]
        public DateTime Created { get; set; }

        public DateTime? Updated { get; set; } = null;

        public DateTime? LastAccessed { get; set; } = null;

        [Required]
        public DateTime NonEditable { get; set; }
    }
}
