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

namespace Fido2me.Pages.OidcApp
{
    public class DeleteModel : PageModel
    {
        private readonly Fido2me.Data.DataContext _context;

        public DeleteModel(Fido2me.Data.DataContext context)
        {
            _context = context;
        }

        [BindProperty]
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

        public async Task<IActionResult> OnPostAsync(Guid? id)
        {
            if (id == null || _context.OidcBasicClients == null)
            {
                return NotFound();
            }
            var oidcbasicclient = await _context.OidcBasicClients.FindAsync(id);

            if (oidcbasicclient != null)
            {
                //OidcBasicClient = oidcbasicclient;
                //_context.OidcBasicClients.Remove(OidcBasicClient);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
