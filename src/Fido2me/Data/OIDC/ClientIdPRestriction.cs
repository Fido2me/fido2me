using System.ComponentModel.DataAnnotations;

namespace Fido2me.Data.OIDC
{
    public class ClientIdPRestriction
    {
        [Required]
        [MaxLength(200)]
        public string Provider { get; set; }
    }
}
