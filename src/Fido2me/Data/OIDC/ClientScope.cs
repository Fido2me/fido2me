using System.ComponentModel.DataAnnotations;

namespace Fido2me.Data.OIDC
{
    public class ClientScope
    {
        [Required]
        [MaxLength(200)]
        public string Scope { get; set; }      
    }
}
