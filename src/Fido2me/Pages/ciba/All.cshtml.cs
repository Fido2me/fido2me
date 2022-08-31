// Based on Duende Sample
// https://github.com/DuendeSoftware/Samples/blob/main/IdentityServer/v6/UserInteraction/Ciba/IdentityServer/Pages/Ciba/All.cshtml.cs

using System.ComponentModel.DataAnnotations;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Fido2me.Pages.ciba;

public class AllModel : PageModel
{
    public IEnumerable<BackchannelUserLoginRequest> Logins { get; set; }

    [BindProperty, Required]
    public string Id { get; set; }
    [BindProperty, Required]
    public string Button { get; set; }

    private readonly IBackchannelAuthenticationInteractionService _backchannelAuthenticationInteraction;

    public AllModel(IBackchannelAuthenticationInteractionService backchannelAuthenticationInteractionService)
    {
        _backchannelAuthenticationInteraction = backchannelAuthenticationInteractionService;
    }

    public async Task OnGet()
    {
        Logins = await _backchannelAuthenticationInteraction.GetPendingLoginRequestsForCurrentUserAsync();
    }
}