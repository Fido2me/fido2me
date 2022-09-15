using Duende.IdentityServer.Models;
using Duende.IdentityServer.Stores;
using Fido2me.Data;
using Fido2me.Data.OIDC;
using Microsoft.EntityFrameworkCore;

namespace Fido2me.Duende
{
    public class PersistedGrantStore : IPersistedGrantStore
    {
        private readonly DataContext _dataContext;
        private readonly ILogger<PersistedGrantStore> _logger;

        public PersistedGrantStore(DataContext dataContext, ILogger<PersistedGrantStore> logger)
        {
            _dataContext = dataContext;
            _logger = logger;
        }

        public Task<IEnumerable<PersistedGrant>> GetAllAsync(PersistedGrantFilter filter)
        {
            
            throw new NotImplementedException();
        }

        public async Task<PersistedGrant> GetAsync(string key)
        {
            var pGrant = await _dataContext.OidcPersistedGrants
                                .AsNoTracking()
                                .Where(g => g.Key == key && g.ConsumedAt == null)
                                .Select(g => new PersistedGrant()
                                {
                                    Key = g.Key,
                                    ClientId = g.ClientId,
                                    CreationTime = g.CreationTime,
                                    Data = g.Data,
                                    Expiration = g.Expiration,
                                    SubjectId = g.SubjectId,
                                    Type = g.Type,
                                })
                                .FirstOrDefaultAsync();

            return pGrant;
        }

        public Task RemoveAllAsync(PersistedGrantFilter filter)
        {
            return Task.CompletedTask;           
        }

        public async Task RemoveAsync(string key)
        {
            var grant = await _dataContext.OidcPersistedGrants.Where(g => g.Key == key && g.ConsumedAt == null).FirstOrDefaultAsync();
            grant.ConsumedAt = DateTimeOffset.UtcNow;

            await _dataContext.SaveChangesAsync();        
        }

        public async Task StoreAsync(PersistedGrant grant)
        {
            var pGrant = new OidcPersistedGrant()
            {
                Key = grant.Key,
                ClientId= grant.ClientId,
                CreationTime = grant.CreationTime,
                Data = grant.Data,
                Expiration = grant.Expiration,
                SubjectId = grant.SubjectId,
                Type = grant.Type,
            };
            await _dataContext.OidcPersistedGrants.AddAsync(pGrant);
            await _dataContext.SaveChangesAsync();            
        }
    }
}
