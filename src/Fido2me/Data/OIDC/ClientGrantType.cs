using System.ComponentModel.DataAnnotations;

namespace Fido2me.Data.OIDC
{
    public class ClientGrantType
    {
        [Required]
        [MaxLength(250)]
        public string GrantType { get; set; }      
    }
}
