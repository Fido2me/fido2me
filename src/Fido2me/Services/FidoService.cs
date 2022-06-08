using Fido2me.Data;
using Fido2me.Data.FIDO2;
using Fido2NetLib.Objects;
using Microsoft.EntityFrameworkCore;

namespace Fido2me.Services
{
    public interface IFidoService
    {
     
        Task AddCredentialAsync(Credential fidoCredential);
        Task<Credential> GetCredentialAsync(byte[] credentialId);

        Task UpdateCredentialAsync(Credential fidoCredential);

        Task CompleteAttestation(BaseAttestationVerification baseAttestationVerification);
    }

    public class FidoService : IFidoService
    {
        private readonly DataContext _context;

        public FidoService(DataContext context)
        {
            _context = context;
        }

   

        public async Task AddCredentialAsync(Credential fidoCredential)
        {
            await _context.Credentials.AddAsync(fidoCredential);
            await _context.SaveChangesAsync();

        }

        public async Task<Credential> GetCredentialAsync(byte[] credentialId)
        {
            var credential = await _context.Credentials.FirstOrDefaultAsync(c => c.Id == credentialId);
            return credential;
        }

        public async Task UpdateCredentialAsync(Credential fidoCredential)
        {
            _context.Credentials.Update(fidoCredential);
            await _context.SaveChangesAsync();
            
        }

        public async Task CompleteAttestation(BaseAttestationVerification attestationResult)
        {
            var credential = new Credential()
            {
                Id = attestationResult.CredentialId,
                AaGuid = attestationResult.Aaguid,
                CredType = attestationResult.CredType,
                PublicKey = attestationResult.PublicKey,
                UserHandle = attestationResult.User.Id,
                RegDate = DateTimeOffset.Now,
                SignatureCounter = attestationResult.Counter,
                AccountId = new Guid(attestationResult.User.Id),
                AttestionResult = attestationResult.AttestationVerificationStatus.ToString()
            };
            await _context.Credentials.AddAsync(credential);

            if (attestationResult.AttestationVerificationStatus == AttestationVerificationStatus.SuccessNoAttestation && attestationResult is LazyAttestationVerificationSuccess)
            {
                var rawAttestation = (attestationResult as LazyAttestationVerificationSuccess).AttestationRawResponse;

                var attestation = new Attestation()
                {
                    Id = rawAttestation.Id,
                    RawId = rawAttestation.RawId, 
                    AttestationObject = rawAttestation.Response.AttestationObject,
                    ClientDataJson = rawAttestation.Response.ClientDataJson
                 
                };
                await _context.Attestations.AddAsync(attestation);
            }

            await _context.SaveChangesAsync();

        }
    }
}
