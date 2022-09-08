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

        [BindProperty]
        public OidcBasicClientViewModel OidcBasicClientVM { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(string clientType)
        {
            switch (clientType)
            {
                case "public":
                    ClientType = "public";
                    break;
                default:
                    ClientType = "private";
                    break;
            }
            var r = await _oidcService.GenerateNewClientIdAndSecret(clientType);
            return Page();
        }
        

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid || OidcBasicClientVM == null)
            {
                return Page();
            }


            await _oidcService.AddBasicClientAsync(OidcBasicClientVM);

            return RedirectToPage("./Index");
        }
    }
}
