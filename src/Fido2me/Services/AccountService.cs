using Fido2me.Data;
using Fido2me.Data.FIDO2;
using Microsoft.EntityFrameworkCore;

namespace Fido2me.Services
{
    public interface IAccountService
    {
        Task<Account> CreateChallengeAsync(string displayName);
        Task AddCredentialAsync(Credential fidoCredential);
        Task<Credential> GetCredentialAsync(byte[] credentialId);

        Task<Account> GetAccountAsync(Guid accountId);

        Task UpdateCredentialAsync(Credential fidoCredential);
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

        public async Task<Account> GetAccountAsync(Guid accountId)
        {
            var account = await _context.Accounts.FirstOrDefaultAsync(a => a.Id == accountId);
            return account;
        }
    }
}
