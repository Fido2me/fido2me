using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using IdentityModel;
using IdentityModel.Client;
using Duende.IdentityServer.Models;
using Fido2me.Services;

namespace Fido2me.Pages.auth
{
    public class CibaLoginModel : PageModel
    {
        private readonly ICibaService _cibaService;

        public CibaLoginModel(ICibaService cibaService)
        {
            _cibaService = cibaService;
        }

        public async Task OnGetAsync()
        {
        }

        public async Task<IActionResult> OnPostAsync([FromForm] string username)
        {
            var usernameNormalized = username.Trim().ToLowerInvariant();
            var loginResponse = await _cibaService.CibaLoginStartAsync(usernameNormalized);

            if (loginResponse.IsError) throw new Exception(loginResponse.Error);

            //Console.WriteLine($"Login Hint                  : {username}");
            //Console.WriteLine($"Binding Message             : {bindingMessage}");
            //Console.WriteLine($"Authentication Request Id   : {response.AuthenticationRequestId}");
            //Console.WriteLine($"Expires In                  : {response.ExpiresIn}");
            //Console.WriteLine($"Interval                    : {response.Interval}");



            return Page();
        }     
    }
}
