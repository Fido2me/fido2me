using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fido2me.Data.OIDC.ConfigurationDb
{
    [Table("ApiResourceClaims")]
    [Index(nameof(ApiResourceId), nameof(Type), IsUnique = true)]
    public class ApiResourceClaim
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int ApiResourceId { get; set; }

        [Required]
        [StringLength(200)]
        public string Type { get; set; }

        ApiResource ApiResource { get; set; } //CONSTRAINT [FK_ApiResourceClaims_ApiResources_ApiResourceId] FOREIGN KEY ([ApiResourceId])
    }
}
