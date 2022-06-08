using System.ComponentModel.DataAnnotations;

namespace Fido2me.Data.OIDC
{
    public class ClientSecret
    {
        [MaxLength(2000)]
        public string Description { get; set; }

        [Required]
        [MaxLength(4000)]
        public string Value { get; set; }

        public DateTime? Expiration { get; set; }

        [Required]
        [MaxLength(250)]
        public string Type { get; set; }

        [Required]
        public DateTime Created { get; set; }       
    }
}
