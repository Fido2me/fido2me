using Fido2me.Data.FIDO2;
using Fido2me.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Fido2me.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IAccountService _accountService;

        [BindProperty]
        public Account Account { get; set; } = default!;

        public IndexModel(ILogger<IndexModel> logger, IAccountService accountService)
        {
            _logger = logger;
            _accountService = accountService;
        }

        public async Task OnGetAsync()
        {
            var accountId = new Guid(User.FindFirst(c => c.Type == "sub").Value);
            Account = await _accountService.GetAccountAsync(accountId);

        }
    }
}