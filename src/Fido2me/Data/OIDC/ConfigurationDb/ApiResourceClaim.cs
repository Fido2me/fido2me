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
        public long Id { get; set; }

        [Required]
        public long ApiResourceId { get; set; }

        [Required]
        [StringLength(200)]
        public string Type { get; set; }

        public ApiResource ApiResource { get; set; }
    }
}
