using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fido2me.Data.OIDC.ConfigurationDb
{
    [Table("ClientRedirectUris")]
    [Index(nameof(ClientId), nameof(RedirectUri), IsUnique = true)]
    public class ClientRedirectUri
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Required]
        [StringLength(400)]
        public string RedirectUri { get; set; }

        [Required]
        public long ClientId { get; set; }
        public Client Client { get; set; }
    }
}