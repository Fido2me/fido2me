using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Fido2me.Pages
{
    public abstract class BasePageModel : PageModel
    {
        public Guid AccountId { get => new(User.FindFirst(c => c.Type == "sub").Value); }

        public string Username { get => new (User.FindFirst(c => c.Type == "name").Value); }

        public string CredentialId { get => new(User.FindFirst(c => c.Type == "credId").Value); }
    }
}
