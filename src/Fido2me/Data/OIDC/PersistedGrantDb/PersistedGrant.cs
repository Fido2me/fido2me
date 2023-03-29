using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fido2me.Data.OIDC.PersistedGrantDb
{
    [Table("PersistedGrants")]
    [Index(nameof(ConsumedTime))]
    [Index(nameof(Expiration))]
    [Index(nameof(Key))] // cannot use unique index, no index filter available in some databases
    [Index(nameof(SubjectId), nameof(ClientId), nameof(Type))]
    [Index(nameof(SubjectId), nameof(SessionId), nameof(Type))]
    public class PersistedGrant
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [StringLength(200)]
        public string Key { get; set; }

        [Required]
        public string Type { get; set; }

        [StringLength(200)]
        public string SubjectId { get; set; }

        [StringLength(100)]
        public string SessionId { get; set; }

        [Required]
        [StringLength(200)]
        public string ClientId { get; set; }

        [StringLength(200)]
        public string Description { get; set; }

        [Required]
        public DateTime CreationTime { get; set; }

        public DateTime? Expiration { get; set; }

        public DateTime? ConsumedTime { get; set; }

        public string Data { get; set; }
    }
}