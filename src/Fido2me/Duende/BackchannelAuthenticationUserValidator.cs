using Duende.IdentityServer.Validation;
using Fido2me.Data;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Fido2me.Duende
{
    public class BackchannelAuthenticationUserValidator : IBackchannelAuthenticationUserValidator
    {
        private readonly DataContext _context;
        private readonly ILogger<BackchannelAuthenticationUserValidator> _logger;

        public BackchannelAuthenticationUserValidator(DataContext context, ILogger<BackchannelAuthenticationUserValidator> logger)
        {
            _context = context;
            _logger = logger;        
        }



        /// <summary>
        /// Validates the backchannel login request with the provided BackchannelAuthenticationUserValidatorContext for the current request. 
        /// </summary>
        /// <param name="userValidatorContext"></param>
        /// <returns>backchannelAuthenticationUserValidatonResult object</returns>
        public async Task<BackchannelAuthenticationUserValidationResult> ValidateRequestAsync(BackchannelAuthenticationUserValidatorContext userValidatorContext)
        {
            // https://docs.duendesoftware.com/identityserver/v6/reference/validators/ciba_user_validator/
            // account enabled / disabled check?
            var userId = await _context.Accounts.Where(a => a.Username == userValidatorContext.LoginHint).Select(a => a.Id).FirstOrDefaultAsync();
            

            var userGuid = userId.ToString();
            var claims = new Claim[] {
                    new Claim("sub", userGuid)
                };
            var identity = new ClaimsIdentity(claims, "ciba");
            var user = new ClaimsPrincipal(identity);
            var br = new BackchannelAuthenticationUserValidationResult()
            {
                Subject = user
            };

            return br;
        }
    }
}
