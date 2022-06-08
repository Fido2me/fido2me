namespace Fido2NetLib.Objects
{
    /// <summary>
    /// Holds parsed credential data
    /// </summary>
    public class AttestationVerificationSuccess : BaseAttestationVerification
    {
        public AttestationVerificationSuccess()
        {
            AttestationVerificationStatus = AttestationVerificationStatus.Success;
        }
    }
}
