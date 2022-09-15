using Duende.IdentityServer;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Stores;
using Fido2me.Data;
using System.Collections.Generic;
using System.Linq;

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
            // openid,profile,offline_access - that's all that we support
            var apiScopes = new List<ApiScope>()
            {
                new ApiScope("openid2")
            };
            return Task.FromResult(apiScopes.AsEnumerable());
        }

        public Task<IEnumerable<IdentityResource>> FindIdentityResourcesByScopeNameAsync(IEnumerable<string> scopeNames)
        {
            var identityResources = new List<IdentityResource>();
            foreach (var scope in scopeNames)
            {
                switch (scope)
                {
                    case IdentityServerConstants.StandardScopes.OpenId:
                        identityResources.Add(new IdentityResources.OpenId());
                        break;
                    case IdentityServerConstants.StandardScopes.Email:
                        identityResources.Add(new IdentityResources.Email());
                        break;
                    case IdentityServerConstants.StandardScopes.Profile:
                        identityResources.Add(new IdentityResources.Profile());
                        break;
                    default:
                        break;
                }
            }
            return Task.FromResult(identityResources.AsEnumerable());

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
                    new IdentityResources.Email(),
                },
                OfflineAccess = false,
                ApiResources = new ApiResource[] { },
            };
            return Task.FromResult(resoures);
        }
    }
}
