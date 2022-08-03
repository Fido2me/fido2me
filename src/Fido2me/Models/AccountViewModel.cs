using Fido2me.Data.FIDO2;
using System.ComponentModel.DataAnnotations;

namespace Fido2me.Models
{
    public class AccountViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public string OldEmail { get; set; } = "";

        public bool EmailVerified { get; set; }

        public EmailVerification? EmailVerification { get; set; } = null;

        public int DeviceAllCount { get; set; }

        public int DeviceEnabledCount { get; set; }

    }
}
