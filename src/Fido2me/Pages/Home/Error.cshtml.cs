using Duende.IdentityServer.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace Fido2me.Pages.Home
{
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    [IgnoreAntiforgeryToken]
    public class ErrorModel : PageModel
    {
        private readonly IHostingEnvironment _environment;
        private readonly ILogger<ErrorModel> _logger;
        private readonly IIdentityServerInteractionService _interaction;

        [BindProperty]
        public string ErrorMessage { get; set; }

        public ErrorModel(IHostingEnvironment environment, ILogger<ErrorModel> logger, IIdentityServerInteractionService interaction)
        {
            _environment = environment;
            _logger = logger;
            _interaction = interaction;
        }

        public async Task OnGetAsync(string? errorId)
        {
            if (_environment.IsDevelopment())
            {
                // https://github.com/IdentityServer/IdentityServer4/issues/984
                var message = await _interaction.GetErrorContextAsync(errorId);
                if (message != null)
                {
                    ErrorMessage = message.Error;
                }
            }            
        }
    }
}