using Microsoft.AspNetCore.Mvc;
using Fido2me.Services;
using Fido2me.Models;

namespace Fido2me.Pages.profile
{
    public class IndexModel : BasePageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IAccountService _accountService;

        [BindProperty]
        public AccountViewModel Account { get; set; } = default!;

        public IndexModel(ILogger<IndexModel> logger, IAccountService accountService)
        {
            _logger = logger;
            _accountService = accountService;
        }

        public async Task OnGetAsync()
        {
            //var accountId = new Guid(User.FindFirst(c => c.Type == "sub").Value);
            Account = await _accountService.GetAccountAsync(AccountId);
            
        }
    }
}
