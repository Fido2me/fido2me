using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fido2me.Data.OIDC.ConfigurationDb
{
    [Table("IdentityResourceProperties")]
    public class IdentityResourceProperty
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Required]
        public long IdentityResourceId { get; set; }
        public IdentityResource IdentityResource { get; set; }

        [Required]
        [StringLength(250)]
        public string Key { get; set; }

        [Required]
        [StringLength(2000)]
        public string Value { get; set; }        
    }
}
