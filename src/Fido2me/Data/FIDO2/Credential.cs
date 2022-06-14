using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fido2me.Data.FIDO2
{
    public class Credential
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public byte[] Id { get; set; }
        public Guid AccountId { get; set; }
        public bool Enabled { get; set; }
        public string CredentialId { get; set; }

        [Required]
        public string DeviceName { get; set; }

        public string DeviceDisplayName { get; set; }

        public string DeviceDescription { get; set; }     

        public byte[] PublicKey { get; set; }
        public byte[] UserHandle { get; set; }
        public uint SignatureCounter { get; set; }
        public string CredType { get; set; }
        public DateTimeOffset RegDate { get; set; }
        public Guid AaGuid { get; set; }

        public string AttestionResult { get; set; }
    }
}
