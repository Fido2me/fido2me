using Duende.IdentityServer.Models;
using Fido2me.Data;
using Fido2me.Data.FIDO2;
using Fido2me.Data.OIDC;
using Fido2me.Models.Applications;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;

namespace Fido2me.Services
{
    public interface IOidcBasicClientService
    {
        Task AddBasicClientAsync(OidcBasicClientViewModel oidcBasicClientVM);
        Task<OidcBasicClientViewModel> GenerateNewClientIdAndSecret(string clientType);
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

        public async Task<OidcBasicClientViewModel> GenerateNewClientIdAndSecret(string clientType)
        {
            var generatedClientId = Guid.NewGuid().ToString("N").ToUpperInvariant();
            var generatedClientSecret = GenerateCryptoRandomString(32);

            var clientVM = new OidcBasicClientViewModel()
            {
                ClientId = generatedClientId,
                ClientSecret = generatedClientSecret,
            };

            return clientVM;

        }

        private static string GenerateCryptoRandomString(int byteLength)
        {
            var random = RandomNumberGenerator.Create();
            var bytes = new byte[32];
            random.GetNonZeroBytes(bytes);
            return Convert.ToHexString(bytes);
        }

        public async Task<List<OidcBasicClientViewModel>> GetBasicClientsByAccountIdAsync(Guid accountId)
        {
            return await _context.OidcBasicClients
                .AsNoTracking()
                .Where(c => c.AccountId == accountId)
                .Select(c => new OidcBasicClientViewModel
                { 
                    ClientId = c.ClientId,
                    ClientName = c.ClientName,
                    AllowOfflineAccess = c.AllowOfflineAccess,
                    
                }).ToListAsync();

            
        }
    }
}
