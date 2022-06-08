using Fido2me.Services;
using Fido2NetLib;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Fido2me.Pages
{
    public class RegisterModel : PageModel
    {
        private readonly IFidoRegistrationService _fidoRegistration;

        [BindProperty]
        public string RegistrationOptions { get; private set; }

        public RegisterModel(IFidoRegistrationService fidoRegistration)
        {
            _fidoRegistration = fidoRegistration;
        }
        public async Task OnGetAsync()
        {
            var options = await _fidoRegistration.RegistrationStartAsync();
            RegistrationOptions = options.ToJson();
        }

        public async Task<IActionResult> OnPostAsync([FromBody] AuthenticatorAttestationRawResponse attestationResponse)
        {
            var registrationResponse = await _fidoRegistration.RegistrationCompleteAsync(attestationResponse, default(CancellationToken));

            return RedirectToPage("/auth/login");

        }
    }
}
