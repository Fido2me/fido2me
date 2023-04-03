using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fido2me.Data.FIDO2
{
    [Table("Accounts")]
    // index on u
    public class Account
    {
        [Key]
        public long Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string Email { get; set; }

        public bool EmailVerified { get; set; } = false;

        public EmailVerification? EmailVerification { get; set; } = null;

        public int DeviceAllCount { get; set; } = 0;

        public int DeviceEnabledCount { get; set; } = 0;

        [Required]
        public AccountType AccountType { get; set; } = AccountType.Personal;
    }

    public enum AccountType : byte
    {
        Personal = 1,
        Group = 10,
    }
}
