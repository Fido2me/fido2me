using System.ComponentModel.DataAnnotations;

namespace Fido2me.Models
{
    public class DeviceViewModel
    {
        [Required]
        public Guid CredentialId { get; set; }

        public string? DeviceDescription { get; set; }

        public string DeviceName { get; set; }

        public string DeviceDisplayName { get; set; }

        [Required]
        public bool Enabled { get; set; }

        public DateTimeOffset RegDate { get; set; }

    }
}
