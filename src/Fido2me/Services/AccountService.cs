using Fido2me.Data;
using Fido2me.Data.FIDO2;
using Fido2me.Models;
using Fido2me.Responses;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;

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
        Task<bool> UpdateAccountAsync(Guid accountId, AccountViewModel account);
    }

    public class AccountService : IAccountService
    {
        private readonly DataContext _context;
        private readonly IEmailService _emailService;
        private readonly ILogger<AccountService> _logger;

        public AccountService(DataContext context, IEmailService emailService, ILogger<AccountService> logger)
        {
            _context = context; 
            _emailService = emailService;

            _logger = logger;
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

            if (account == null || account.EmailVerification == null || account.EmailVerification.Code == 0)
            {
                return new EmailVerificationResponse(EmailVerificationResponseStatus.Error, "Email verification has not been requested.");
            }

            if (account.EmailVerification.FailedAttempts > 5)
            {
                return new EmailVerificationResponse(EmailVerificationResponseStatus.Error, "Too many failed attemts to verify your email address. Contact support.");
            }

            if (code != account.EmailVerification.Code)
            {
                account.EmailVerification.Code = 0; // nulify the challenge
                account.EmailVerification.FailedAttempts++;
                await _context.SaveChangesAsync();
                return new EmailVerificationResponse(EmailVerificationResponseStatus.Error, "Code mismatch. Start email verification process again.");
            }

            // all good
            account.Email = account.EmailVerification.Email;
            account.EmailVerified = true;
            account.EmailVerification = null;
            await _context.SaveChangesAsync();

            return new EmailVerificationResponse(EmailVerificationResponseStatus.Success, "All good");
        }

        public async Task<bool> UpdateAccountAsync(Guid accountId, AccountViewModel accountVM)
        {
            if (accountVM.OldEmail?.ToLower().Trim() == accountVM.Email.ToLower().Trim())
            {
                // this one will return false if initial attempt was unsuccessful, but you still want to use the email, so this code needs to be rewritten
                // nothing to change at this point
                return false;
            }
            
            var account = await _context.Accounts.FirstOrDefaultAsync(a => a.Id == accountId);
            if (account == null)
                throw new ArgumentNullException("Account not found");

            account.Email = accountVM.Email.ToLower().Trim();
            account.EmailVerified = false;

            var code = GenerateChallengeCode();
            if (account.EmailVerification == null)
            {
                // initial challenge request (first ever or after successful email verification)
                account.EmailVerification = new EmailVerification()
                {
                    Created = DateTimeOffset.Now,
                    Email = account.Email,
                    FailedAttempts = 0,
                    Code = code
                };
            }
            else
            {
                // account.EmailVerification.Created vs DateTimeOffset.Now + 1 day

                if (account.Email == accountVM.Email.ToLower().Trim())
                {
                    // not first attempt for this email - legit use case?
                }

                account.EmailVerification.Code = code;
                account.EmailVerification.Email = account.Email;
                // do not change failed attempts here
                account.EmailVerification.Created = DateTimeOffset.Now;
            }
            await _context.SaveChangesAsync();

            var emailSent = await _emailService.SendEmailAsync(account.Email, code);

            return emailSent;
        }

        private int GenerateChallengeCode()
        {
            var random = RandomNumberGenerator.GetInt32(1000000, int.MaxValue);
            return random;
        }
    }
}
