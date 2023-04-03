using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Fido2me.Pages
{
    public abstract class BasePageModel : PageModel
    {
        public long AccountId => long.Parse(User.FindFirst(c => c.Type == "sub").Value);

        public string Username => User.FindFirst(c => c.Type == "name").Value;

        public string CredentialId => User.FindFirst(c => c.Type == "credId").Value;
    }
}
