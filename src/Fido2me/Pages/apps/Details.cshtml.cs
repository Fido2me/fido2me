using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Fido2me.Data;
using Fido2me.Data.OIDC;
using Fido2me.Models.Applications;

namespace Fido2me.Pages.OidcApp
{
    public class DetailsModel : PageModel
    {
        private readonly Fido2me.Data.DataContext _context;

        public DetailsModel(Fido2me.Data.DataContext context)
        {
            _context = context;
        }

      public OidcBasicClientViewModel OidcBasicClient { get; set; } = default!; 

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null || _context.OidcBasicClients == null)
            {
                return NotFound();
            }

            var oidcbasicclient = await _context.OidcBasicClients.FirstOrDefaultAsync(m => m.Id == id);
            if (oidcbasicclient == null)
            {
                return NotFound();
            }
            else 
            {
                //OidcBasicClient = oidcbasicclient;
            }
            return Page();
        }
    }
}
