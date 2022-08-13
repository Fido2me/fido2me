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
            // validate email address
            if (!EmailHelper.IsValidEmail(authCheck.Username))
            {
                return new JsonResult(new CredentialCreateOptions() 
                { 
                    Status = "Invalid email format.",
                    ErrorMessage = "Invalid email format."
                });
            }

            var options = await _fidoRegistration.RegistrationStartAsync(authCheck.Username, authCheck.IsResident);
            
            return new JsonResult(options);
        }

       
    }

    public class AuthCheck
    {
        public bool IsResident { get; set; }
        public string Username { get; set; }

    }
}
