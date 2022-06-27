using Fido2me.Models;
using Fido2me.Services;
using Microsoft.AspNetCore.Mvc;

namespace Fido2me.Pages
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
            Account = await _accountService.GetAccountAsync(AccountId);


        }
    }
}