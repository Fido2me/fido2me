using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;
using Fido2me.Data;
using Microsoft.EntityFrameworkCore;

namespace Fido2me.Duende
{
    // https://www.scottbrady91.com/identity-server/creating-your-own-identityserver4-storage-library

    public class CorsPolicyService : ICorsPolicyService
    {
        private readonly DataContext _dataContext;
        public CorsPolicyService(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<bool> IsOriginAllowedAsync(string origin)
        {
            return true;
        }
    }
}
