using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fido2me.Data.FIDO2
{
    [Table("Assertions")]
    public class Assertion
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Required]
        public byte[] RawId { get; set; }

        [Required]
        public long CredentialId { get; set; }

        [Required]
        public DateTime Created { get; set; } = DateTime.UtcNow;

        public string AuthenticatorData { get; set; }
        public string ClientDataJson { get; set; }

        public string Signature { get; set; }

        public string UserHandle { get; set; }
    }
}
