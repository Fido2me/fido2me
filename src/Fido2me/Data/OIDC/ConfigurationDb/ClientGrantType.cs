using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fido2me.Data.OIDC.ConfigurationDb
{
    [Table("ClientGrantTypes")]
    [Index(nameof(ClientId), nameof(GrantType), IsUnique = true)]
    public class ClientGrantType
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Required]
        [StringLength(250)]
        public string GrantType { get; set; }

        [Required]
        public long ClientId { get; set; }

        public Client Client { get; set; }
    }
}
