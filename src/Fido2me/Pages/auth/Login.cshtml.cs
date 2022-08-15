using Duende.IdentityServer;
using Fido2me.Services;
using Fido2NetLib;
using Fido2NetLib.Objects;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using System.Text.Json;

namespace Fido2me.Pages
{
    public class LoginModel : PageModel
    {
        private readonly ILogger<LoginModel> _logger;

        [BindProperty(SupportsGet = true)]
        public string? ReturnUrl { get; set; }

        [BindProperty]
        public string AssertionOptions { get; private set; }

        [TempData]
        public string ErrorMessage { get; set; }

        private readonly IFidoLoginService _fidoLogin;


        public LoginModel(ILogger<LoginModel> logger, IFidoLoginService fidoLogin,  IDataProtectionProvider provider)
        {
            _logger = logger;   
            _fidoLogin = fidoLogin;   
        }

        public async Task OnGetAsync()
        {
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }

            ReturnUrl = ReturnUrl ?? Url.Content("~/");

            // Clear the existing external cookie to ensure a clean login process
            // await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme).ConfigureAwait(false);
        }

        public async Task<JsonResult> OnPostCheckAsync([FromBody] AuthCheck authCheck)
        {
            var username = authCheck.Username.Trim().ToLowerInvariant();
            // https://www.w3.org/TR/webauthn-2/#enum-residentKeyRequirement
            var options = await _fidoLogin.LoginStartAsync(username);

            return new JsonResult(options);
        }

        public async Task<IActionResult> OnPostAsync([FromForm] string assertionResponse)
        {
            var assertionResponseJson = JsonSerializer.Deserialize<AuthenticatorAssertionRawResponse>(assertionResponse);
            
            ReturnUrl = ReturnUrl ?? Url.Content("~/");

            var loginResponse = await _fidoLogin.LoginCompleteAsync(assertionResponseJson, default(CancellationToken));

            if (loginResponse.LoginResponseStatus == Responses.LoginResponseStatus.Success)
            {
                var claims = new Claim[]
                {
                    new Claim("credId", loginResponse.CredentialId),
                    new Claim("name", loginResponse.DeviceName),
                    new Claim("displayName", loginResponse.DeviceDisplayName),
                    new Claim("aaGuid", loginResponse.AaGuid.ToString()),
                };

                var user = new IdentityServerUser(loginResponse.AccountId.ToString())
                {
                    AdditionalClaims = claims,
                };

                _logger.LogInformation("User logged in.");
                await HttpContext.SignInAsync(user).ConfigureAwait(false);

                if (Url.IsLocalUrl(ReturnUrl))
                {
                    return Redirect(ReturnUrl);
                }
            }

            ErrorMessage = loginResponse.ErrorMessage;
            ModelState.AddModelError(string.Empty, loginResponse.ErrorMessage);
            //return new JsonResult("Invalid login attempt.");        
            return RedirectToPage("./Login", new { ReturnUrl = ReturnUrl});


        }
    }
}