using Duende.IdentityServer.Models;
using Duende.IdentityServer.Stores;

namespace Fido2me.Duende
{
    public class BackChannelAuthenticationRequestStore : IBackChannelAuthenticationRequestStore
    {
        public Task<string> CreateRequestAsync(BackChannelAuthenticationRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<BackChannelAuthenticationRequest> GetByAuthenticationRequestIdAsync(string requestId)
        {
            throw new NotImplementedException();
        }

        public Task<BackChannelAuthenticationRequest> GetByInternalIdAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<BackChannelAuthenticationRequest>> GetLoginsForUserAsync(string subjectId, string clientId = null)
        {
            throw new NotImplementedException();
        }

        public Task RemoveByInternalIdAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateByInternalIdAsync(string id, BackChannelAuthenticationRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
