using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fido2me.Data.OIDC.PersistedGrantDb
{
    [Table("DeviceCodes")]
    [Index(nameof(DeviceCode), IsUnique = true)]
    [Index(nameof(Expiration))]
    public class DeviceCodeFlow
    {
        [Key]
        [Required]
        [StringLength(200)]
        public string UserCode { get; set; }

        [Required]
        [StringLength(200)]
        public string DeviceCode { get; set; }

        [Required]
        [StringLength(200)]
        public string SubjectId { get; set; }

        [Required]
        [StringLength(100)]
        public string SessionId { get; set; }

        [Required]
        [StringLength(200)]
        public string ClientId { get; set; }

        [StringLength(200)]
        public string Description { get; set; }

        [Required]
        public DateTime CreationTime { get; set; }

        [Required]
        public DateTime Expiration { get; set; }

        [Required]
        public string Data { get; set; }
    }
}
