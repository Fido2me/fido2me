using Fido2me.Helpers;
using Fido2me.Services;
using Fido2NetLib;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;

namespace Fido2me.Pages
{
    public class RegisterModel : PageModel
    {
        private readonly IFidoRegistrationService _fidoRegistration;

       
        public RegisterModel(IFidoRegistrationService fidoRegistration)
        {
            _fidoRegistration = fidoRegistration;
        }

        public async Task<IActionResult> OnPostAsync([FromForm] string attestationResponse)
        {
            
            var attestationResponseJson = JsonSerializer.Deserialize<AuthenticatorAttestationRawResponse>(attestationResponse);
            var registrationResponse = await _fidoRegistration.RegistrationCompleteAsync(attestationResponseJson, default(CancellationToken));

            return RedirectToPage("/auth/login");

        }


        public async Task<JsonResult> OnPostCheckAsync([FromBody] AuthCheck authCheck)
        {
            /* expect username only
            if (!EmailHelper.IsValidEmail(authCheck.Username))
            {
                return new JsonResult(new CredentialCreateOptions() 
                { 
                    Status = "Invalid email format.",
                    ErrorMessage = "Invalid email format."
                });
            }
            */

            var username = authCheck.Username.Trim().ToLowerInvariant();
            // https://www.w3.org/TR/webauthn-2/#enum-residentKeyRequirement
            var options = await _fidoRegistration.RegistrationStartAsync(username, false);
            
            return new JsonResult(options);
        }

       
    }

    public class AuthCheck
    {
        public string Username { get; set; }
    }
}
