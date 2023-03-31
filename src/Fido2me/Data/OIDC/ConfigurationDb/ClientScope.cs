using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fido2me.Data.OIDC.ConfigurationDb
{
    [Table("ClientScopes")]
    [Index(nameof(ClientId), nameof(Scope), IsUnique = true)]
    public class ClientScope
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Scope { get; set; }

        [Required]
        public long ClientId { get; set; }
        public Client Client { get; set; }
    }
}