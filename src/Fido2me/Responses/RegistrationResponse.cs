using Fido2NetLib.Objects;

namespace Fido2me.Responses
{
    public class RegistrationResponse
    {
        public AttestationVerificationSuccess AttestationVerificationStatus { get; private set; }

        public string ErrorMessage { get; set; } = string.Empty;
        public RegistrationStatus RegistrationStatus { get; set; }

        public RegistrationResponse(RegistrationStatus registrationStatus, AttestationVerificationSuccess attestationVerificationSuccess = null)
        {
            AttestationVerificationStatus = attestationVerificationSuccess;
            RegistrationStatus = registrationStatus;
        }
    }
    
    public enum RegistrationStatus
    {
        Success,
        Failure,
    }
}
