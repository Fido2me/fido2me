using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;

namespace Fido2me.Duende
{
    public class BackchannelAuthenticationUserNotificationService : IBackchannelAuthenticationUserNotificationService
    {
        /// <summary>
        /// Sends a notification for the user to login via the BackchannelUserLoginRequest parameter.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public Task SendLoginRequestAsync(BackchannelUserLoginRequest request)
        {
            return Task.FromResult(0);
        }
    }
}
