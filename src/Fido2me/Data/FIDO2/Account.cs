using System.ComponentModel.DataAnnotations;

namespace Fido2me.Data.FIDO2
{
    public class Account
    {
        [Key]
        public Guid Id { get; set; }

        public string AccountType => "User";
 

        public string Email { get; set; }



        public Account() { }
    }
}
