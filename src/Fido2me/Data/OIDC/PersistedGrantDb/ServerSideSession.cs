using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fido2me.Data.OIDC.PersistedGrantDb
{
    [Table("ServerSideSessions")]
    [Index(nameof(DisplayName))]
    [Index(nameof(Expires))]
    [Index(nameof(Key), IsUnique = true)]
    [Index(nameof(SessionId))]
    [Index(nameof(SubjectId))]
    public class ServerSideSession
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Key { get; set; }

        [Required]
        [StringLength(100)]
        public string Scheme { get; set; }

        [Required]
        [StringLength(100)]
        public string SubjectId { get; set; }

        [StringLength(100)]
        public string SessionId { get; set; }

        [StringLength(100)]
        public string DisplayName { get; set; }

        [Required]
        public DateTime Created { get; set; } = DateTime.UtcNow;

        [Required]
        public DateTime Renewed { get; set; }

        public DateTime Expires { get; set; }

        [Required]
        public string Data { get; set; }
    }
}