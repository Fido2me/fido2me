using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fido2me.Data.FIDO2
{
    public class Assertion
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public byte[] Id { get; set; }

        [Required]
        public byte[] RawId { get; set; }

        public string AuthenticatorData { get; set; }
        public string ClientDataJson { get; set; }

        public string Signature { get; set; }

        public string UserHandle { get; set; }

        public string Type => "public-key";

        public Assertion() { }
    }
}
