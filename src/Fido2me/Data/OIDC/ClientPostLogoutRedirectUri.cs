using System.ComponentModel.DataAnnotations;

namespace Fido2me.Data.OIDC
{
    public class ClientPostLogoutRedirectUri
    {
        [Required]
        [MaxLength(400)]
        public string PostLogoutRedirectUri { get; set; }       
    }
}
