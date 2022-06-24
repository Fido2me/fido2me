using Microsoft.AspNetCore.Mvc;
using Fido2me.Services;
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
            Account.OldEmail = Account.Email;
            
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync([FromForm] AccountViewModel account)
        {
            ModelState.Remove("Account.OldEmail");
            if (ModelState.IsValid)
            {
                // Save changes
                var requireEmailVerification = await _accountService.UpdateAccountAsync(AccountId, account);

                if (requireEmailVerification)
                {
                    // need to verify email again
                    return RedirectToPage("./EmailVerification");
                }
            }
            return Page();
        }
    }
}
