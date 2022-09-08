using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Fido2me.Data;
using Fido2me.Data.OIDC;
using Fido2me.Models.Applications;

namespace Fido2me.Pages.OidcApp
{
    public class EditModel : PageModel
    {
        private readonly Fido2me.Data.DataContext _context;

        public EditModel(Fido2me.Data.DataContext context)
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

            var oidcbasicclient =  await _context.OidcBasicClients.FirstOrDefaultAsync(m => m.Id == id);
            if (oidcbasicclient == null)
            {
                return NotFound();
            }
            //OidcBasicClient = oidcbasicclient;
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

            _context.Attach(OidcBasicClient).State = EntityState.Modified;

            await _context.SaveChangesAsync();


            return RedirectToPage("./Index");
        }

        private bool OidcBasicClientExists(Guid id)
        {
          return (_context.OidcBasicClients?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
