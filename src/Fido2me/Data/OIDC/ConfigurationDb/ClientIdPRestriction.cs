using Duende.IdentityServer.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fido2me.Data.OIDC.ConfigurationDb
{
    [Table("ClientIdPRestrictions")]
    [Index(nameof(ClientId), nameof(Provider), IsUnique = true)]
    public class ClientIdPRestriction
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Provider { get; set; }

        [Required]
        public long ClientId { get; set; }

        public Client Client { get; set; }
    }
}
