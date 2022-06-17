using Fido2me.Data;
using Fido2me.Data.FIDO2;
using Fido2me.Models;
using Fido2me.Responses;
using Microsoft.EntityFrameworkCore;

namespace Fido2me.Services
{
    public interface IAccountService
    {
        Task<Account> CreateChallengeAsync(string displayName);
        Task AddCredentialAsync(Credential fidoCredential);
        Task<Credential> GetCredentialAsync(byte[] credentialId);

        Task<AccountViewModel> GetAccountAsync(Guid accountId);

        Task UpdateCredentialAsync(Credential fidoCredential);
        Task<EmailVerificationResponse> VerifyEmailAsync(Guid accountId, int code);
    }

    public class AccountService : IAccountService
    {
        private readonly DataContext _context;

        public AccountService(DataContext context)
        {
            _context = context;
        }

        public async Task<Account> CreateChallengeAsync(string displayName)
        {
            var account = await _context.Accounts.FirstOrDefaultAsync();
            return account;
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

        public async Task<AccountViewModel> GetAccountAsync(Guid accountId)
        {
            var accountVM = await _context.Accounts.Where(a => a.Id == accountId).AsNoTracking()
                .Select(a => new AccountViewModel() 
                {  
                    Email = a.Email,
                    EmailVerified = a.EmailVerified,
                    EmailVerification = a.EmailVerification,
                    DeviceAllCount = a.DeviceAllCount,
                    DeviceEnabledCount = a.DeviceEnabledCount,
                }).FirstOrDefaultAsync();
            return accountVM;
        }

        public async Task<EmailVerificationResponse> VerifyEmailAsync(Guid accountId, int code)
        {
            var account = await _context.Accounts.FirstOrDefaultAsync(a => a.Id == accountId);

            if (account == null || account.EmailVerification == null)
            {
                return new EmailVerificationResponse(EmailVerificationResponseStatus.Error, "Email verification has not been requested.");
            }

            if (code != account.EmailVerification.Code)
            {
                account.EmailVerification = null; // nulify the challenge
                await _context.SaveChangesAsync();
                return new EmailVerificationResponse(EmailVerificationResponseStatus.Error, "Code mismatch. Start email verification method again.");
            }

            // all good
            account.Email = account.EmailVerification.Email;
            account.EmailVerified = true;
            account.EmailVerification = null;
            await _context.SaveChangesAsync();

            return new EmailVerificationResponse(EmailVerificationResponseStatus.Success, "All good");
        }
    }
}
