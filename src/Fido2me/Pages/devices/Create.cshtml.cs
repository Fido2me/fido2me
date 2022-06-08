using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Fido2me.Data;
using Fido2me.Data.FIDO2;

namespace Fido2me.Pages.devices
{
    public class CreateModel : PageModel
    {
        private readonly Fido2me.Data.DataContext _context;

        public CreateModel(Fido2me.Data.DataContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Credential Credential { get; set; } = default!;
        

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
          if (!ModelState.IsValid || _context.Credentials == null || Credential == null)
            {
                return Page();
            }

            _context.Credentials.Add(Credential);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
