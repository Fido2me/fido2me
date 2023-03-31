using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fido2me.Data.OIDC.ConfigurationDb
{
    [Table("ApiResourceScopes")]
    [Index(nameof(ApiResourceId), nameof(Scope), IsUnique = true)]
    public class ApiResourceScope
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Scope { get; set; }

        [Required]
        public long ApiResourceId { get; set; }

        public ApiResource ApiResource { get; set; }
    }
}
