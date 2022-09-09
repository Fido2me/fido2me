using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Fido2me.Services;
using Microsoft.AspNetCore.Components;
using Fido2me.Models.Applications;

namespace Fido2me.Pages.OidcApp
{
    public class CreateModel : BasePageModel
    {
        private readonly IOidcBasicClientService _oidcService;

        public CreateModel(IOidcBasicClientService oidcService)
        {
            _oidcService = oidcService;
        }

        public string ClientType { get; set; }

        [TempData]
        public string GeneratedSecretData { get; set; }

        [BindProperty]
        public OidcCreateClientViewModel OidcCreateClientViewModel { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync()
        {

            OidcCreateClientViewModel = await _oidcService.GenerateNewClientIdAndSecret();
            GeneratedSecretData = OidcCreateClientViewModel.ClientId + ":" + OidcCreateClientViewModel.ClientSecret;
            return Page();
        }
        

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid || OidcCreateClientViewModel == null)
            {
                return Page();
            }

            OidcCreateClientViewModel.Scope = GenerateScopeString(OidcCreateClientViewModel.ClientScopes);
            var generatedSecrets = GeneratedSecretData.Split(':');
            // no tampering for client id and client secret
            OidcCreateClientViewModel.ClientId = generatedSecrets[0];
            OidcCreateClientViewModel.ClientSecret = generatedSecrets[1];

            await _oidcService.AddBasicClientAsync(OidcCreateClientViewModel, AccountId);

            return RedirectToPage("./Index");
        }

        private string GenerateScopeString(ClientScopes clientScopes)
        {
            var scopes = "openid";
            if (clientScopes.Email)
            {
                scopes += " email";
            };
            if (clientScopes.Profile)
            {
                scopes += " profile";
            }
            return scopes;
        }
    }
}
