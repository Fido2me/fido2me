// Based on Duende Software Sample
// https://github.com/DuendeSoftware/Samples/blob/main/IdentityServer/v6/UserInteraction/Ciba/IdentityServer/Pages/Ciba/ViewModel.cs

namespace Fido2me.Pages.ciba
{
    public class ViewModel
    {
        public string ClientName { get; set; }
        public string ClientUrl { get; set; }
        public string ClientLogoUrl { get; set; }

        public string BindingMessage { get; set; }

        public IEnumerable<ScopeViewModel> IdentityScopes { get; set; }
        public IEnumerable<ScopeViewModel> ApiScopes { get; set; }
    }

    public class ScopeViewModel
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }
        public bool Emphasize { get; set; }
        public bool Required { get; set; }
        public bool Checked { get; set; }
        public IEnumerable<ResourceViewModel> Resources { get; set; }
    }

    public class ResourceViewModel
    {
        public string Name { get; set; }
        public string DisplayName { get; set; }
    }
}
