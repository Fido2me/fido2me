using Duende.IdentityServer.Models;
using Duende.IdentityServer.Stores;
using Fido2me.Data;
using System.Collections.Generic;

namespace Fido2me.Duende
{
    // we are not using resource part currently
    // https://github.com/IdentityServer/IdentityServer4.EntityFramework.Storage/tree/dev/src/Stores

    //https://github.com/DuendeSoftware/IdentityServer/blob/ffa4da997b44dcb32103bb26a919fb14cd1cf0f5/src/IdentityServer/Extensions/IResourceStoreExtensions.cs

    public class ResourceStore : IResourceStore
    {
        private readonly DataContext _dataContext;
        private readonly ILogger<ResourceStore> _logger;

        public ResourceStore(DataContext dataContext, ILogger<ResourceStore> logger)
        {
            _dataContext = dataContext;
            _logger = logger;
        }

        public Task<IEnumerable<ApiResource>> FindApiResourcesByNameAsync(IEnumerable<string> apiResourceNames)
        {
            return Task.FromResult(Enumerable.Empty<ApiResource>());
        }

        public Task<IEnumerable<ApiResource>> FindApiResourcesByScopeNameAsync(IEnumerable<string> scopeNames)
        {
            return Task.FromResult(Enumerable.Empty<ApiResource>());
        }

        public Task<IEnumerable<ApiScope>> FindApiScopesByNameAsync(IEnumerable<string> scopeNames)
        {
            return Task.FromResult(Enumerable.Empty<ApiScope>());
        }

        public Task<IEnumerable<IdentityResource>> FindIdentityResourcesByScopeNameAsync(IEnumerable<string> scopeNames)
        {
            return Task.FromResult(
                new List<IdentityResource>()
                {
                    new IdentityResources.OpenId(),
                    new IdentityResources.Profile(),
                }.AsEnumerable<IdentityResource>()
            );
        }

        public Task<Resources> GetAllResourcesAsync()
        {
            var resoures = new Resources()
            {
                ApiScopes = new ApiScope[] { },
                IdentityResources =  
                new IdentityResource[]
                {
                    new IdentityResources.OpenId(),
                    new IdentityResources.Profile(),
                },
                OfflineAccess = true,
                ApiResources = new ApiResource[] { },
            };
            return Task.FromResult(resoures);
        }
    }
}
