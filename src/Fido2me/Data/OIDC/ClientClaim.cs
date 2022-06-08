using System.ComponentModel.DataAnnotations;

namespace Fido2me.Data.OIDC
{
    public class ClientClaim
    {
        [Required]
        [MaxLength(250)]
        public string Type { get; set; }

        [Required]
        [MaxLength(250)]
        public string Value { get; set; }
    }
}
