using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fido2me.Data.OIDC.ConfigurationDb
{
    [Table("ApiResourceProperties")]
    [Index(nameof(ApiResourceId), nameof(Key), IsUnique = true)]
    public class ApiResourceProperty
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Required]
        public long ApiResourceId { get; set; }

        [Required]
        [StringLength(250)]
        public string Key { get; set; }

        [Required]
        [StringLength(2000)]
        public string Value { get; set; }

        public ApiResource ApiResource { get; set; }
    }     
}
