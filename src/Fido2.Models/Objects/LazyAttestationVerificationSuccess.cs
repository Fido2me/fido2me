namespace Fido2NetLib.Objects
{
    /// <summary>
    /// Holds parsed credential data
    /// </summary>
    public class LazyAttestationVerificationSuccess : BaseAttestationVerification
    {
        public AuthenticatorAttestationRawResponse AttestationRawResponse { get; set; }
        
        public LazyAttestationVerificationSuccess()
        {
            AttestationVerificationStatus = AttestationVerificationStatus.SuccessNoAttestation;

        }
    }
}
