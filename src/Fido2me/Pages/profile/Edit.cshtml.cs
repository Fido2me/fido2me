using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Fido2me.Data;
using Fido2me.Data.FIDO2;
using Fido2me.Services;
using Fido2me.Pages.Shared;
using Fido2me.Models;

namespace Fido2me.Pages.profile
{
    public class EditModel : BasePageModel
    {
        private readonly IAccountService _accountService;
        private readonly ILogger<EditModel> _logger;

        public EditModel(IAccountService accountService, ILogger<EditModel> logger)
        {
            _accountService = accountService;
            _logger = logger;
        }

        [BindProperty]
        public AccountViewModel Account { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync()
        {
            Account = await _accountService.GetAccountAsync(AccountId);
            
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync([FromForm] AccountViewModel account)
        {

            return Page();
        }
    }
}
