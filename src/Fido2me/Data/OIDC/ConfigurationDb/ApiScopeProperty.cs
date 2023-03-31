using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fido2me.Data.OIDC.ConfigurationDb
{
    [Table("ApiScopeProperties")]
    [Index(nameof(ApiScopeId), nameof(Key), IsUnique = true)]
    public class ApiScopeProperty
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Required]
        public long ApiScopeId { get; set; }
        public ApiScope ApiScope { get; set; } // [FK_ApiScopeProperties_ApiScopes_ScopeId] FOREIGN KEY ([ScopeId]) REFERENCES [ApiScopes] ([Id])

        [Required]
        [StringLength(250)]
        public string Key { get; set; }

        [Required]
        [StringLength(2000)]
        public string Value { get; set; }
    }
}