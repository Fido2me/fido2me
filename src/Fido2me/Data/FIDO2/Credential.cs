using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fido2me.Data.FIDO2
{
    [Table("Credentials")]
    public class Credential
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Required]
        public long AccountId { get; set; }

        [Required]
        public bool Enabled { get; set; }

        [Required]
        public byte[] CredentialId { get; set; }

        [NotMapped]
        public string CredentialIdString => Convert.ToHexString(CredentialId);

        [Required]
        public string Username { get; set; }

        public string Nickname { get; set; }

        public string DeviceDescription { get; set; }     

        public byte[] PublicKey { get; set; }
        public byte[] UserHandle { get; set; }
        public uint SignatureCounter { get; set; }
        public string CredType { get; set; }
        public DateTime RegDate { get; set; }
        public Guid AaGuid { get; set; }

        public IList<Assertion> Assertions { get; set; }
    }
}
