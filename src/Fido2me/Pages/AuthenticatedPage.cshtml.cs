using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Fido2me.Pages
{
    [Authorize]
    public class AuthenticatedPageModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
