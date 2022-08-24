using Duende.IdentityServer.Validation;
using System.Security.Claims;

namespace Fido2me.Duende
{
    public class BackchannelAuthenticationUserValidator : IBackchannelAuthenticationUserValidator
    {
        /// <summary>
        /// Validates the backchannel login request with the provided BackchannelAuthenticationUserValidatorContext for the current request. 
        /// </summary>
        /// <param name="userValidatorContext"></param>
        /// <returns>backchannelAuthenticationUserValidatonResult object</returns>
        public Task<BackchannelAuthenticationUserValidatonResult> ValidateRequestAsync(BackchannelAuthenticationUserValidatorContext userValidatorContext)
        {
            // https://docs.duendesoftware.com/identityserver/v6/reference/validators/ciba_user_validator/
            var userGuid = Guid.NewGuid().ToString();
            var claims = new Claim[] {
                    new Claim("sub", userGuid)
                };
            var identity = new ClaimsIdentity(claims, "ciba");
            var user = new ClaimsPrincipal(identity);
            var br = new BackchannelAuthenticationUserValidatonResult()
            {
                Subject = user
            };
            return Task.FromResult(br);
        }
    }
}
