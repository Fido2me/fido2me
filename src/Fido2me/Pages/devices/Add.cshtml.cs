using Fido2me.Services;
using Fido2NetLib;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;

namespace Fido2me.Pages.devices
{
    public class AddModel : BasePageModel
    {
        private readonly IFidoRegistrationService _fidoRegistration;
        private readonly ILogger<AddModel> _logger;

        [BindProperty]
        public string AddOptions { get; private set; }

        public AddModel(IFidoRegistrationService fidoRegistration, ILogger<AddModel> logger)
        {
            _fidoRegistration = fidoRegistration;
            _logger = logger;
        }

        public async Task OnGetAsync()
        {
            /*
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }

            ReturnUrl = ReturnUrl ?? Url.Content("~/");
            */

            var createOptions = await _fidoRegistration.RegistrationAddNewDeviceStartAsync(Username);
            AddOptions = createOptions.ToJson();

        }

        public async Task<IActionResult> OnPostAsync([FromForm] string attestationResponse)
        {

            var attestationResponseJson = JsonSerializer.Deserialize<AuthenticatorAttestationRawResponse>(attestationResponse);
            var registrationResponse = await _fidoRegistration.RegistrationAddNewDeviceCompleteAsync(attestationResponseJson, AccountId, default(CancellationToken));

            return RedirectToPage("/devices/Index");

        }
    }
}
