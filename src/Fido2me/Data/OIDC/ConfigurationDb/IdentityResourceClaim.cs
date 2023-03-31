using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fido2me.Data.OIDC.ConfigurationDb
{
    [Table("IdentityResourceClaims")]
    [Index(nameof(IdentityResourceId), nameof(Type), IsUnique = true)]
    public class IdentityResourceClaim
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Required]
        public int IdentityResourceId { get; set; }
        public IdentityResource IdentityResource { get; set; } //CONSTRAINT [FK_IdentityResourceClaims_IdentityResources_IdentityResourceId] FOREIGN KEY ([IdentityResourceId])

        [Required]
        [StringLength(200)]
        public string Type { get; set; }
    }
}
