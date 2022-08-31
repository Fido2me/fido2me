using Duende.IdentityServer.Models;
using Duende.IdentityServer.Stores;
using Fido2me.Data;

namespace Fido2me.Duende
{
    public class PersistedGrantStore : IPersistedGrantStore
    {
        private readonly DataContext _dataContext;
        public PersistedGrantStore(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public Task<IEnumerable<PersistedGrant>> GetAllAsync(PersistedGrantFilter filter)
        {
            
            throw new NotImplementedException();
        }

        public Task<PersistedGrant> GetAsync(string key)
        {
            throw new NotImplementedException();
        }

        public Task RemoveAllAsync(PersistedGrantFilter filter)
        {
            throw new NotImplementedException();
        }

        public Task RemoveAsync(string key)
        {
            throw new NotImplementedException();
        }

        public Task StoreAsync(PersistedGrant grant)
        {
            return Task.CompletedTask;
            throw new NotImplementedException();
        }
    }
}
