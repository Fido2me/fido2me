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

namespace Fido2me.Pages.OidcApp
{
    public class IndexModel : BasePageModel
    {
        private readonly IOidcBasicClientService _oidcService;

        [BindProperty]
        public List<OidcBasicClientViewModel> OidcBasicClientVMs { get; set; } = default!;

        public IndexModel(IOidcBasicClientService oidcService)
        {
            _oidcService = oidcService;
        }

        public async Task OnGetAsync()
        {
            OidcBasicClientVMs = await _oidcService.GetBasicClientsByAccountIdAsync(AccountId);

        }
    }
}
