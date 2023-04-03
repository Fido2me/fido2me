using Duende.IdentityServer;
using Duende.IdentityServer.Models;
using Fido2me.Data;
using Fido2me.Data.OIDC;
using Fido2me.Models.Applications;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using OidcModel = Fido2me.Data.OIDC.ConfigurationDb;

namespace Fido2me.Services
{
    public interface IOidcBasicClientService
    {
        Task AddBasicClientAsync(OidcCreateClientViewModel oidcBasicClientVM, long accountId);
        Task ChangeClientStatusAsync(string clientId, long accountId);
        Task DeleteClientAsync(string clientId, long accountId);
        Task<bool> EditClientAsync(OidcClientEditViewModel oidcBasicClient, long accountId);
        Task<OidcCreateClientViewModel> GenerateNewClientIdAndSecret();
        Task<List<OidcClientIndexViewModel>> GetBasicClientsByAccountIdAsync(long accountId);
        Task<OidcClientEditViewModel> GetClientToEditAsync(string clientId, long accountId);
    }

    public class OidcBasicClientService : IOidcBasicClientService
    {
        private readonly ApplicationDataContext _context;

        private const int ClientSecretLength = 32;

        public OidcBasicClientService(ApplicationDataContext context)
        {
            _context = context;
        }

        public async Task AddBasicClientAsync(OidcCreateClientViewModel oidcBasicClientVM, long accountId)
        {
            var currentTime = DateTime.UtcNow;
            // allow to set PKCE mode only for confidential clients, public clients will default to true (RequireClientSecret = Confidential cLient)
            var requirePkce = oidcBasicClientVM.RequireClientSecret ? oidcBasicClientVM.RequirePkce : true;

            var oidcClient = new OidcModel.Client()
            {
                AccountId = accountId,
                ClientId = oidcBasicClientVM.ClientId,

                RequireClientSecret = oidcBasicClientVM.RequireClientSecret,
                ClientSecrets =
                {
                    new OidcModel.ClientSecret()
                    {
                        Created = currentTime,
                        Type = IdentityServerConstants.SecretTypes.SharedSecret,
                        Value = oidcBasicClientVM.ClientSecret.Sha256(),
                    }
                },
                AllowOfflineAccess = false, // delegate session management completely to relying party?
                Enabled = true,
                AllowRememberConsent = true,
                RequireConsent = true,
                RequirePkce = requirePkce,
                ClientName = oidcBasicClientVM.ClientName,
                Description = oidcBasicClientVM.Description,
                Created = currentTime,

                //ClientGrantTypes = GrantTypes.Code.ToArray(), // use code + PKCE for both public and confidential clients
                // ClientScopes = oidcBasicClientVM.Scope.Split(' '),
                // ClientRedirectUris = new string[] { oidcBasicClientVM.RedirectUri },
            };
            
            //oidcBasicClient.ClientGrantTypes.Add(new ClientGrantType() { GrantType = "" });
            //if (oidcBasicClientVM.RequireClientSecret)
            {
                //oidcBasicClient.ClientCorsOrigins = new string[1] { oidcBasicClientVM.CorsOrigin };
            }

            await _context.Clients.AddAsync(oidcClient);
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

        public async Task<List<OidcClientIndexViewModel>> GetBasicClientsByAccountIdAsync(long accountId)
        {
            return await _context.Clients
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

        public async Task<bool> EditClientAsync(OidcClientEditViewModel oidcClientEdit, long accountId)
        {
            var client = await _context.Clients
                .Include(c => c.ClientRedirectUris)
                .FirstOrDefaultAsync(c => c.AccountId == accountId && c.ClientId == oidcClientEdit.Id);
            if (client == null)
            {
                return false;
            }
            var requirePkce = client.RequireClientSecret ? oidcClientEdit.RequirePkce : true;
            client.Updated = DateTime.UtcNow;
            client.ClientName = oidcClientEdit.Name;
            client.Description = oidcClientEdit.Description;
            //client.ClientRedirectUris = new string[] { oidcClientEdit.RedirectUri };
            client.Enabled = oidcClientEdit.Enabled;
            //client.ClientCorsOrigins = new string[] { oidcClientEdit.CorsOrigin };
            client.RequirePkce = requirePkce;

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<OidcClientEditViewModel> GetClientToEditAsync(string clientId, long accountId)
        {
            var client = await _context.Clients
                .AsNoTracking()
                .Where(c => c.AccountId == accountId && c.ClientId == clientId)
                .Select(c => new OidcClientEditViewModel
                {
                    Id = c.ClientId,
                    Name = c.ClientName,
                    Description = c.Description,
                    Enabled = c.Enabled,
                    Type = c.RequireClientSecret ? "Confidential" : "Public",
                    //Scopes = c.ClientScopes,
                    //RedirectUris = c.ClientRedirectUris,
                    //CorsOrigins = c.ClientCorsOrigins,
                    RequirePkce = c.RequirePkce,

                }).FirstOrDefaultAsync();

            client.Scope = string.Join(" ", client.Scopes);
            client.RedirectUri = string.Join(" ", client.RedirectUris);
            //client.CorsOrigin = string.Join(" ", client.CorsOrigins);
            return client;
        }

        public async Task DeleteClientAsync(string clientId, long accountId)
        {
            var client = await _context.Clients.FirstOrDefaultAsync(c => c.ClientId == clientId && c.AccountId == accountId);
            if (client != null)
            {
                _context.Clients.Remove(client);
                await _context.SaveChangesAsync();
            }
        }

        public async Task ChangeClientStatusAsync(string clientId, long accountId)
        {
            var client = await _context.Clients.FirstOrDefaultAsync(c => c.ClientId == clientId && c.AccountId == accountId);
            if (client != null)
            {
                client.Enabled = !client.Enabled;
                await _context.SaveChangesAsync();
            }
        }
    }
}
