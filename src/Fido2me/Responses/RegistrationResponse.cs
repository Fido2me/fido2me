using Fido2NetLib.Objects;

namespace Fido2me.Responses
{
    public class RegistrationResponse
    {
        public AttestationVerificationStatus AttestationVerificationStatus { get; private set; }

        public string ErrorMessage { get; set; } = string.Empty;

        public RegistrationResponse(AttestationVerificationStatus attestationVerificationStatus)
        {
            AttestationVerificationStatus = attestationVerificationStatus;
        }
    } 
}
