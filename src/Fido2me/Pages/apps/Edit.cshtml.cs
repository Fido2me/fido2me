using Microsoft.AspNetCore.Mvc;
using Fido2me.Data;
using Fido2me.Models.Applications;
using Fido2me.Services;

namespace Fido2me.Pages.OidcApp
{
    public class EditModel : BasePageModel
    {
        private readonly DataContext _context;

        private readonly IOidcBasicClientService _oidcService;

        public EditModel(DataContext context, IOidcBasicClientService oidcService)
        {
            _context = context;
            _oidcService = oidcService;
        }

        [BindProperty]
        public OidcClientEditViewModel OidcClientEdit { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(string? clientId)
        {
            if (clientId == null)
            {
                return NotFound();
            }

            OidcClientEdit =  await _oidcService.GetClientToEditAsync(clientId, AccountId);
            if (OidcClientEdit == null)
            {
                return NotFound();
            }
            
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var c = await _oidcService.EditClientAsync(OidcClientEdit, AccountId);

            return RedirectToPage("./Index");
        }

        private bool OidcBasicClientExists(string id)
        {
          return (_context.OidcBasicClients?.Any(e => e.ClientId == id)).GetValueOrDefault();
        }
    }
}
