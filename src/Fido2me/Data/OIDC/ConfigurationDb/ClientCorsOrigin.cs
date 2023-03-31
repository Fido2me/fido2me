using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fido2me.Data.OIDC.ConfigurationDb
{
    [Table("ClientCorsOrigins")]
    [Index(nameof(ClientId), nameof(Origin), IsUnique = true)]
    public class ClientCorsOrigin
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Required]
        [StringLength(150)]
        public string Origin { get; set; }

        [Required]
        public long ClientId { get; set; }
        public Client Client { get; set; }
    }
}