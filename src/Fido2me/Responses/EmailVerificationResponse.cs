namespace Fido2me.Responses
{
    public class EmailVerificationResponse
    {
        

        public EmailVerificationResponseStatus EmailVerificationResponseStatus { get; private set; }

        public string ErrorMessage { get; set; } = string.Empty;

        public EmailVerificationResponse(EmailVerificationResponseStatus emailVerificationResponseStatus, string errorMessage)
        {
            EmailVerificationResponseStatus = emailVerificationResponseStatus;
            ErrorMessage = errorMessage;
        }
    }

    public enum EmailVerificationResponseStatus
    {
        Error = 0,
        Success = 1,
    }
}
