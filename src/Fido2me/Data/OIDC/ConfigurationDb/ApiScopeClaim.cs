using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fido2me.Data.OIDC.ConfigurationDb
{
    [Table("ApiScopeClaims")]
    [Index(nameof(ApiScopeId), nameof(Type), IsUnique = true)]
    public class ApiScopeClaim
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Required]
        public long ApiScopeId { get; set; }
        public ApiScope ApiScope { get; set; }


        [Required]
        [StringLength(200)]
        public string Type { get; set; }
    }
}
