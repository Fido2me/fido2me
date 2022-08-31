// Based on Duende Software Sample
// https://github.com/DuendeSoftware/Samples/blob/main/IdentityServer/v6/UserInteraction/Ciba/IdentityServer/Pages/Ciba/Index.cshtml.cs

using System.Threading.Tasks;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace Fido2me.Pages.ciba;

[AllowAnonymous]
public class IndexModel : PageModel
{
    public BackchannelUserLoginRequest LoginRequest { get; set; }

    private readonly IBackchannelAuthenticationInteractionService _backchannelAuthenticationInteraction;
    private readonly ILogger<IndexModel> _logger;

    public IndexModel(IBackchannelAuthenticationInteractionService backchannelAuthenticationInteractionService, ILogger<IndexModel> logger)
    {
        _backchannelAuthenticationInteraction = backchannelAuthenticationInteractionService;
        _logger = logger;
    }

    public async Task<IActionResult> OnGet(string id)
    {
        LoginRequest = await _backchannelAuthenticationInteraction.GetLoginRequestByInternalIdAsync(id);
        if (LoginRequest == null)
        {
            _logger.LogWarning("Invalid backchannel login id {id}", id);
            return RedirectToPage("/general/error");
        }

        return Page();
    }
}