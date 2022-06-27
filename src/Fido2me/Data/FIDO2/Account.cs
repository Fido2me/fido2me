using System.ComponentModel.DataAnnotations;

namespace Fido2me.Data.FIDO2
{
    public class Account
    {
        [Key]
        public Guid Id { get; set; }

        public string AccountType => "User";


        public string Email { get; set; } = "";

        public bool EmailVerified { get; set; } = false;

        public EmailVerification? EmailVerification { get; set; } = null;

        public int DeviceAllCount { get; set; } = 0;

        public int DeviceEnabledCount { get; set; } = 0;

        public Account() { }
    }
}
