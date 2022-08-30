using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Fido2me.Services;

namespace Fido2me.Pages.auth
{
    public class CibaLoginModel : PageModel
    {
        private readonly ICibaService _cibaService;

        [BindProperty]
        public CibaLoginViewModel CibaLoginVM { get; set; }

        public CibaLoginModel(ICibaService cibaService)
        {
            _cibaService = cibaService;
        }

        public async Task OnGetAsync()
        {
            var cibaVm = _cibaService.GetAuthenticationRequestDetails();

            // request expired, go to external login again
            if (cibaVm == null)
            {
                CibaLoginVM = null;
                return;
            }

            CibaLoginVM = cibaVm;
        }

        public async Task<IActionResult> OnPostAsync([FromForm] string username)
        {
            var usernameNormalized = username.Trim().ToLowerInvariant();
            var bindingMessage = Guid.NewGuid().ToString("N").Substring(0, 10);

            var loginResponse = await _cibaService.CibaLoginStartAsync(usernameNormalized, bindingMessage);

            // TODO: issue model error instead
            if (loginResponse.IsError)
                throw new Exception(loginResponse.Error);

            CibaLoginVM = new CibaLoginViewModel()
            {
                Message = bindingMessage,
                Username = usernameNormalized,
            };

            return Page();
        }



    }
    
    public class CibaLoginViewModel
    {
        public string Username { get; set; }
        public string RequestId { get; set; }
        public string Message { get; set; }
    }
}
