namespace Fido2me.Responses
{
    public class LoginResponse
    {
        public long AccountId { get; set; }

        public string CredentialId { get; set; }

        public Guid AaGuid { get; set; }

        public string Username { get; set; }

       
        public LoginResponseStatus LoginResponseStatus { get; private set; }

        public string ErrorMessage { get; set; } = string.Empty;

        public LoginResponse(LoginResponseStatus loginResponseStatus)
        {
            LoginResponseStatus = loginResponseStatus;
        }
    }

    public enum LoginResponseStatus
    {
        Error = 0,
        Success = 1,
    }
}
