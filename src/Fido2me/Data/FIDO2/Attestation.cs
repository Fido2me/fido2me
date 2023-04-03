using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fido2me.Data.FIDO2
{
    [Table("Attestations")]
    public class Attestation
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Required]
        public byte[] RawId { get; set; }

        [Required]
        public byte[] AttestationObject { get; set; }

        [Required]
        public byte[] ClientDataJson { get; set; }

        [Required]
        public long CredentialId { get; set; }

        [Required]
        public DateTime Created { get; set; } = DateTime.UtcNow;

        public AttestionResult AttestionResult { get; set; }
    }


    public enum AttestionResult : byte
    {
        Failure = 0,
        Skipped = 10,
        Success = 11,
    }
}
