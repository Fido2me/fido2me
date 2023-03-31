using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fido2me.Data.OIDC.ConfigurationDb
{
    [Table("IdentityResourceProperties")]
    public class IdentityResourceProperty
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int IdentityResourceId { get; set; }
        public IdentityResource IdentityResource { get; set; } //  [FK_IdentityResourceProperties_IdentityResources_IdentityResourceId] FOREIGN KEY ([IdentityResourceId]) REFERENCES [IdentityResources] ([Id])

        [Required]
        [StringLength(250)]
        public string Key { get; set; }

        [Required]
        [StringLength(2000)]
        public string Value { get; set; }        
    }
}
