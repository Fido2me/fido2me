using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Fido2me.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Duende.IdentityServer;
using Fido2me.Services;

namespace Fido2me.Pages.auth
{
    public class CibaLoginModel : BasePageModel
    {
        private readonly ICibaService _cibaService;
        private readonly ILogger<CibaLoginModel> _logger;

        [BindProperty]
        public CibaLoginViewModel CibaLoginVM { get; set; }

        public CibaLoginModel(ICibaService cibaService, ILogger<CibaLoginModel> logger)
        {
            _cibaService = cibaService;
            _logger = logger;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var cibaVm = _cibaService.GetAuthenticationRequestDetails();

            // request expired, go to external login again
            if (cibaVm == null)
            {
                CibaLoginVM = null;
                return Page();
            }

            CibaLoginVM = cibaVm;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync([FromForm] string username)
        {
            var usernameNormalized = username.Trim().ToLowerInvariant();
            var bindingMessage = Guid.NewGuid().ToString("N").Substring(0, 10);

            var loginResponse = await _cibaService.CibaLoginStartAsync(usernameNormalized, bindingMessage);

            // TODO: issue model error instead
            if (loginResponse.IsError)
                throw new Exception(loginResponse.Error);

            CibaLoginVM = loginResponse.CibaLoginVM;

            return Page();
        }


        public async Task<IActionResult> OnPostContinueAsync()
        {
            var cibaVm = _cibaService.GetAuthenticationRequestDetails();
            if (cibaVm == null)
            {
                //CibaLog = new CibaLoginContinueViewModel() { IsError = false, Error = "Expired" };
                return RedirectToPage("./auth/ciba");
            }

            var completeResponse = await _cibaService.CibaTryLoginCompleteAsync(cibaVm.RequestId);

            if (completeResponse.IsError)
            {
                //CibaLoginContinueVM = new CibaLoginContinueViewModel() { IsError = false, Error = "Expired" };
                return Page();
            }
            else
            {
                // TODO: Do we need a token validation (signature validation + fields) here again? Should we trust ourselves?
                var idToken = new JwtSecurityToken(completeResponse.IdentityToken);
                var subject = idToken.Subject;
                var cibaSubject = await _cibaService.GetClaimsToCompleteCibaAsync(subject, cibaVm.RequestId);

                var claims = new Claim[]
                {
                    new Claim("credId", cibaSubject.CredentialId),
                    new Claim("name", cibaSubject.Username),
                    //new Claim("aaGuid", loginResponse.AaGuid.ToString()),
                };

                var user = new IdentityServerUser(idToken.Subject)
                {
                    AdditionalClaims = claims,
                };

                _logger.LogInformation("User logged in as ... with CIBA.");
                await HttpContext.SignInAsync(user).ConfigureAwait(false);

                return Redirect("~/");

            }     

        }

    }
    

}
