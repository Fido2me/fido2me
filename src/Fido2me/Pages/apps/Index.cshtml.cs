using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Fido2me.Data;
using Fido2me.Data.OIDC;
using Fido2me.Models;
using Fido2me.Services;

namespace Fido2me.Pages.OidcApp
{
    public class IndexModel : PageModel
    {
        private readonly IOidcBasicClientService _oidcService;

        public IndexModel(IOidcBasicClientService oidcService)
        {
            _oidcService = oidcService;
        }

        public IList<OidcBasicClientViewModel> OidcBasicClients { get; set; } = default!;

        public async Task OnGetAsync()
        {
            // get accountId
            //User.Claims.
            //OidcBasicClients = await _oidcService.GetBasicClientsByAccountIdAsync()

        }
    }
}
