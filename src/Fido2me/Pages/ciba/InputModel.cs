// Based on Duende Software Sample
// https://github.com/DuendeSoftware/Samples/blob/main/IdentityServer/v6/UserInteraction/Ciba/IdentityServer/Pages/Ciba/InputModel.cs

namespace Fido2me.Pages.ciba
{
    public class InputModel
    {
        public string Button { get; set; }
        public IEnumerable<string> ScopesConsented { get; set; }
        public string Id { get; set; }
        public string Description { get; set; }
    }
}
