using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fido2me.Data.OIDC.ConfigurationDb
{
    [Table("ClientPostLogoutRedirectUris")]
    [Index(nameof(ClientId), nameof(PostLogoutRedirectUri), IsUnique = true)]
    public class ClientPostLogoutRedirectUri
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Required]
        [StringLength(400)]
        public string PostLogoutRedirectUri { get; set; }

        [Required]
        public long ClientId { get; set; }

        public Client Client { get; set; }

    }
}