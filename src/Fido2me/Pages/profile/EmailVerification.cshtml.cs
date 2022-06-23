using Fido2me.Pages.Shared;
using Fido2me.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
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
        public int Code { get; set; }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync([FromForm] int code)
        {
            var r = await _accountService.VerifyEmailAsync(AccountId, code);
            if (r.EmailVerificationResponseStatus == Responses.EmailVerificationResponseStatus.Success)
            {
                return RedirectToPage("./Index");
            }

            // display errors
            return Page();
        }
    }
}
