using Fido2me.Services;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Fido2me.Pages.profile
{
    public class EmailVerificationModel : BasePageModel
    {
        private readonly IAccountService _accountService;
        private readonly ILogger<EmailVerificationModel> _logger;

        public EmailVerificationModel(IAccountService accountService, ILogger<EmailVerificationModel> logger)
        {
            _accountService = accountService;
            _logger = logger;
        }

        [BindProperty]
        [Required]
        public string Code { get; set; }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync([FromForm] string code)
        {
            int.TryParse(code, out int mcode);
            var r = await _accountService.VerifyEmailAsync(AccountId, mcode);
            if (r.EmailVerificationResponseStatus == Responses.EmailVerificationResponseStatus.Success)
            {
                return RedirectToPage("./Index");
            }

            // display errors
            return Page();
        }
    }
}
