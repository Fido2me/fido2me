using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fido2me.Data.OIDC.PersistedGrantDb
{
    [Table("Keys")]
    [Index(nameof(Use))]
    public class Key
    {
        [Key]
        [StringLength(450)]
        public string Id { get; set; }

        [Required]
        public int Version { get; set; }

        [Required]
        public DateTime Created { get; set; }

        [StringLength(450)]
        public string Use { get; set; }

        [Required]
        [StringLength(100)]
        public string Algorithm { get; set; }

        [Required]
        public bool IsX509Certificate { get; set; }

        [Required]
        public bool DataProtected { get; set; }

        [Required]
        public string Data { get; set; }
    }
}
