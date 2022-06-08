using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Fido2me.Data;
using Fido2me.Data.OIDC;
using Fido2me.Services;
using Fido2me.Models;

namespace Fido2me.Pages.OidcApp
{
    public class CreateModel : PageModel
    {
        private readonly IOidcBasicClientService _oidcService;

        public CreateModel(IOidcBasicClientService oidcService)
        {
            _oidcService = oidcService;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public OidcBasicClientViewModel OidcBasicClientVM { get; set; } = default!;
        

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
