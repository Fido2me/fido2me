using Fido2me.Data.FIDO2;

namespace Fido2me.Models
{
    public class AccountViewModel
    {
        public string Email { get; set; }

        public bool EmailVerified { get; set; }

        public EmailVerification? EmailVerification { get; set; } = null;

        public int DeviceAllCount { get; set; }

        public int DeviceEnabledCount { get; set; }

    }
}
