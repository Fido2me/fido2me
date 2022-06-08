using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fido2me.Data.FIDO2
{
    public class Attestation
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public byte[] Id { get; set; }

        [Required]
        public byte[] RawId { get; set; }

        public byte[] AttestationObject { get; set; }

        public byte[] ClientDataJson { get; set; }

        public string Type => "public-key";

        public Attestation() { }
    }
}
