using System.ComponentModel.DataAnnotations;

namespace Fido2me.Models.Applications
{
    public class OidcClientEditViewModel
    {
        [Required]
        public bool Enabled { get; set; }

        public string Id { get; set; }

        public string Type { get; set; }

        public string Scope { get; set; }

        public string[] Scopes { get; set; }

        [Required]
        [MaxLength(200)]
        public string Name { get; set; }

        [MaxLength(1000)]
        public string Description { get; set; }

        [MaxLength(150)]
        public string CorsOrigin { get; set; }

        public string[] CorsOrigins { get; set; }

        [Required]
        [MaxLength(200)]
        public string RedirectUri { get; set; }

        public string[] RedirectUris { get; set; }
    }
}
