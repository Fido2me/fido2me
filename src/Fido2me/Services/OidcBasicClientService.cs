using Duende.IdentityServer.Models;
using Fido2me.Data;
using Fido2me.Data.FIDO2;
using Fido2me.Data.OIDC;
using Fido2me.Models;
using Microsoft.EntityFrameworkCore;

namespace Fido2me.Services
{
    public interface IOidcBasicClientService
    {
        Task AddBasicClientAsync(OidcBasicClientViewModel oidcBasicClientVM);

        Task<List<OidcBasicClientViewModel>> GetBasicClientsByAccountIdAsync(Guid accountId);
    }

    public class OidcBasicClientService : IOidcBasicClientService
    {
        private readonly DataContext _context;

        public OidcBasicClientService(DataContext context)
        {
            _context = context;
        }

        public async Task AddBasicClientAsync(OidcBasicClientViewModel oidcBasicClientVM)
        {
            var oidcBasicClient = new OidcBasicClient()
            {
                
            };
            oidcBasicClient.ClientGrantTypes = GrantTypes.Code.ToArray();
            //oidcBasicClient.ClientGrantTypes.Add(new ClientGrantType() { GrantType = "" });
            if (oidcBasicClientVM.RequireClientSecret)
            {
                oidcBasicClient.ClientCorsOrigins = new string[1] { oidcBasicClientVM.CorsOrigin };
            }

            await _context.OidcBasicClients.AddAsync(oidcBasicClient);
            await _context.SaveChangesAsync();
        }

        public async Task<List<OidcBasicClientViewModel>> GetBasicClientsByAccountIdAsync(Guid accountId)
        {
            return await _context.OidcBasicClients
                .Where(c => c.AccountId == accountId).AsNoTracking()
                .Select(c => new OidcBasicClientViewModel { ClientId = c.ClientId }).ToListAsync();

            
        }
    }
}
