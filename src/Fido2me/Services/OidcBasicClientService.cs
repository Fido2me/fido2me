using Duende.IdentityServer;
using Duende.IdentityServer.Models;
using Fido2me.Data;
using Fido2me.Data.FIDO2;
using Fido2me.Data.OIDC;
using Fido2me.Models.Applications;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client.Platforms.Features.DesktopOs.Kerberos;
using System.Security.Cryptography;

namespace Fido2me.Services
{
    public interface IOidcBasicClientService
    {
        Task AddBasicClientAsync(OidcCreateClientViewModel oidcBasicClientVM, Guid accountId);
        Task ChangeClientStatusAsync(string clientId, Guid accountId);
        Task DeleteClientAsync(string clientId, Guid accountId);
        Task<bool> EditClientAsync(OidcClientEditViewModel oidcBasicClient, Guid accountId);
        Task<OidcCreateClientViewModel> GenerateNewClientIdAndSecret();
        Task<List<OidcClientIndexViewModel>> GetBasicClientsByAccountIdAsync(Guid accountId);
        Task<OidcClientEditViewModel> GetClientToEditAsync(string clientId, Guid accountId);
    }

    public class OidcBasicClientService : IOidcBasicClientService
    {
        private readonly DataContext _context;

        private const int ClientSecretLength = 32;

        public OidcBasicClientService(DataContext context)
        {
            _context = context;
        }

        public async Task AddBasicClientAsync(OidcCreateClientViewModel oidcBasicClientVM, Guid accountId)
        {
            var currentTime = DateTimeOffset.UtcNow;

            var oidcBasicClient = new OidcBasicClient()
            {
                AccountId = accountId,

                ClientId = oidcBasicClientVM.ClientId,
                ClientSecrets = { new ClientSecret()
                {
                    Created = currentTime,
                    Type = IdentityServerConstants.SecretTypes.SharedSecret,
                    Value = oidcBasicClientVM.ClientSecret.Sha256(),
                } },
                AllowOfflineAccess = false, // delegate session management completely to relying party?
                ClientGrantTypes = GrantTypes.Code.ToArray(), // use code + PKCE for both public and confidential clients
                Enabled = true,
                AllowRememberConsent = true,
                RequireConsent = true,
                ClientName = oidcBasicClientVM.ClientName,
                Description = oidcBasicClientVM.Description,
                Created = currentTime,
                RequireClientSecret = oidcBasicClientVM.RequireClientSecret,
                ClientScopes = oidcBasicClientVM.Scope.Split(' '),
                ClientRedirectUris = new string[] { oidcBasicClientVM.RedirectUri },
            };
            
            //oidcBasicClient.ClientGrantTypes.Add(new ClientGrantType() { GrantType = "" });
            //if (oidcBasicClientVM.RequireClientSecret)
            {
                //oidcBasicClient.ClientCorsOrigins = new string[1] { oidcBasicClientVM.CorsOrigin };
            }

            await _context.OidcBasicClients.AddAsync(oidcBasicClient);
            await _context.SaveChangesAsync();
        }

        public async Task<OidcCreateClientViewModel> GenerateNewClientIdAndSecret()
        {
            var generatedClientId = Guid.NewGuid().ToString("N").ToLowerInvariant();
            var generatedClientSecret = GenerateCryptoRandomString(ClientSecretLength).ToLowerInvariant();

            var clientVM = new OidcCreateClientViewModel()
            {
                ClientId = generatedClientId,
                ClientSecret = generatedClientSecret,
            };

            return clientVM;

        }

        private static string GenerateCryptoRandomString(int byteLength)
        {
            var random = RandomNumberGenerator.Create();
            var bytes = new byte[byteLength];
            random.GetNonZeroBytes(bytes);
            return Convert.ToHexString(bytes);
        }

        public async Task<List<OidcClientIndexViewModel>> GetBasicClientsByAccountIdAsync(Guid accountId)
        {
            return await _context.OidcBasicClients
                .AsNoTracking()
                .Where(c => c.AccountId == accountId)
                .Select(c => new OidcClientIndexViewModel
                { 
                    Id = c.ClientId,
                    Name = c.ClientName,
                    Description = c.Description,
                    Enabled = c.Enabled,
                    Type = c.RequireClientSecret ? "Confidential" : "Public",                 
                    
                }).ToListAsync();
        }

        public async Task<bool> EditClientAsync(OidcClientEditViewModel oidcClientEdit, Guid accountId)
        {
            var client = await _context.OidcBasicClients.FirstOrDefaultAsync(c => c.AccountId == accountId && c.ClientId == oidcClientEdit.Id);

            client.Updated = DateTimeOffset.UtcNow;
            client.ClientName = oidcClientEdit.Name;
            client.Description = oidcClientEdit.Description;
            client.ClientRedirectUris = new string[] { oidcClientEdit.RedirectUri };
            client.Enabled = oidcClientEdit.Enabled;
            client.ClientCorsOrigins = new string[] { oidcClientEdit.CorsOrigin };

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<OidcClientEditViewModel> GetClientToEditAsync(string clientId, Guid accountId)
        {
            var client = await _context.OidcBasicClients
                .AsNoTracking()
                .Where(c => c.AccountId == accountId)
                .Select(c => new OidcClientEditViewModel
                {
                    Id = c.ClientId,
                    Name = c.ClientName,
                    Description = c.Description,
                    Enabled = c.Enabled,
                    Type = c.RequireClientSecret ? "Confidential" : "Public",
                    Scopes = c.ClientScopes,
                    RedirectUris = c.ClientRedirectUris,
                    CorsOrigins = c.ClientCorsOrigins,

                }).FirstOrDefaultAsync();

            client.Scope = string.Join(" ", client.Scopes);
            client.RedirectUri = string.Join(" ", client.RedirectUris);
            //client.CorsOrigin = string.Join(" ", client.CorsOrigins);
            return client;
        }

        public async Task DeleteClientAsync(string clientId, Guid accountId)
        {
            var client = await _context.OidcBasicClients.FirstOrDefaultAsync(c => c.ClientId == clientId && c.AccountId == accountId);
            if (client != null)
            {
                _context.OidcBasicClients.Remove(client);
                await _context.SaveChangesAsync();
            }
        }

        public async Task ChangeClientStatusAsync(string clientId, Guid accountId)
        {
            var client = await _context.OidcBasicClients.FirstOrDefaultAsync(c => c.ClientId == clientId && c.AccountId == accountId);
            if (client != null)
            {
                client.Enabled = !client.Enabled;
                await _context.SaveChangesAsync();
            }
        }
    }
}
