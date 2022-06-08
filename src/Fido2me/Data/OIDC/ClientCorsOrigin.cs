using System.ComponentModel.DataAnnotations;

namespace Fido2me.Data.OIDC
{
    public class ClientCorsOrigin
    {
        [Required]
        [MaxLength(150)]
        public string Origin { get; set; }

        public ClientCorsOrigin(string origin)
        {
            Origin = origin;
        }
    }
}
