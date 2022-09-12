using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Fido2me.Data;
using Fido2me.Data.OIDC;
using Fido2me.Services;
using Fido2me.Models.Applications;
using Fido2me.Models;

namespace Fido2me.Pages.OidcApp
{
    public class IndexModel : BasePageModel
    {
        private readonly IOidcBasicClientService _oidcService;

        [BindProperty]
        public List<OidcClientIndexViewModel> OidcClientIndexVMs { get; set; } = default!;

        public IndexModel(IOidcBasicClientService oidcService)
        {
            _oidcService = oidcService;
        }

        public async Task OnGetAsync()
        {
            OidcClientIndexVMs = await _oidcService.GetBasicClientsByAccountIdAsync(AccountId);

        }

        public async Task<IActionResult> OnPostEnableAsync(string clientId)
        {
            await _oidcService.ChangeClientStatusAsync(clientId, AccountId);
            return RedirectToPage("./Index");
        }

        public async Task<IActionResult> OnPostDeleteAsync(string clientId)
        {
            await _oidcService.DeleteClientAsync(clientId, AccountId);
            return RedirectToPage("./Index");
        }
    }
}
