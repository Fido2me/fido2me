using Duende.IdentityServer.Models;
using Duende.IdentityServer.Stores;
using Fido2me.Data;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Fido2me.Duende
{
    public class BackChannelAuthenticationRequestStore : IBackChannelAuthenticationRequestStore
    {
        private readonly DataContext _context;
        private readonly ILogger<BackChannelAuthenticationRequestStore> _logger;

        public BackChannelAuthenticationRequestStore(DataContext context, ILogger<BackChannelAuthenticationRequestStore> logger)
        {
            _context = context;
            _logger = logger;
        }   

        public async Task<string> CreateRequestAsync(BackChannelAuthenticationRequest request)
        {
            var cibaLoginRequest = new Fido2me.Data.OIDC.ciba.CibaLoginRequest()
            {
                InternalId = Guid.NewGuid().ToString("N"),
                BindingMessage = request.BindingMessage,
                IdP = request.IdP,
                SubjectId = request.Subject.FindFirst(c => c.Type == "sub").Value,
                Tenant = request.Tenant,                
            };
            var r = await _context.CibaLoginRequests.AddAsync(cibaLoginRequest);
            await _context.SaveChangesAsync();

            return "success?";
        }

        public Task<BackChannelAuthenticationRequest> GetByAuthenticationRequestIdAsync(string requestId)
        {
            throw new NotImplementedException();
        }

        public Task<BackChannelAuthenticationRequest> GetByInternalIdAsync(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<BackChannelAuthenticationRequest>> GetLoginsForUserAsync(string subjectId, string clientId = null)
        {
            var claims = new Claim[] {
                    new Claim("sub", subjectId)
                };
            var identity = new ClaimsIdentity(claims, "ciba");
            var subject = new ClaimsPrincipal(identity);
   

            var loginRequests = await _context.CibaLoginRequests
                .Where(c => c.SubjectId == subjectId)
                .Select(c => new BackChannelAuthenticationRequest() 
                {
                    BindingMessage = c.BindingMessage,
                    InternalId = c.InternalId,
                    Subject = subject, // if no entries, our subject doesn't matter, so reuse the parameter
                    ClientId = clientId, // from db?
                    
                    
                })
                .ToListAsync();

            return loginRequests;
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
