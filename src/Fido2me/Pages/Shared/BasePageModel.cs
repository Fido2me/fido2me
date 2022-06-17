using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Fido2me.Pages.Shared
{
    public abstract class BasePageModel : PageModel
    {
        public Guid AccountId { get => new(User.FindFirst(c => c.Type == "sub").Value); }
    }
}
