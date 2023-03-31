using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fido2me.Data.OIDC.ConfigurationDb
{
    [Table("ClientClaims")]
    [Index(nameof(ClientId), nameof(Type), nameof(Value), IsUnique = true)]
    public class ClientClaim
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Required]
        [StringLength(250)]
        public string Type { get; set; }

        [Required]
        [StringLength(250)]
        public string Value { get; set; }

        [Required]
        public long ClientId { get; set; }
        public Client Client { get; set; }
    }
}
