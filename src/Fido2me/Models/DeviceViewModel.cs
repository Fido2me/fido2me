using System.ComponentModel.DataAnnotations;

namespace Fido2me.Models
{
    public class DeviceViewModel
    {
        [Required]
        public string CredentialId { get; set; }

        public string? DeviceDescription { get; set; }

        public string Username { get; set; }

        [Required]
        public bool Enabled { get; set; }

        public DateTimeOffset RegDate { get; set; }

    }
}
