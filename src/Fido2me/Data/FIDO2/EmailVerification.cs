namespace Fido2me.Data.FIDO2
{
    public class EmailVerification
    {
        public string Email { get; set; } = "";
        public DateTimeOffset? Created { get; set; }
        public int? Code { get; set; }
    }
}
