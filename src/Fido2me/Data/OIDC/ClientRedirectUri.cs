using System.ComponentModel.DataAnnotations;

namespace Fido2me.Data.OIDC
{
    public class ClientRedirectUri
    {
        [Required]
        [MaxLength(400)]
        public string RedirectUri { get; set; }   
    }
}
