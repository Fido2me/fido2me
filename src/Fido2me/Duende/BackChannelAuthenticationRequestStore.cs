using Duende.IdentityServer.Models;
using Duende.IdentityServer.Stores;
using Fido2me.Data;
using Fido2me.Helpers;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Fido2me.Duende
{
    public class BackChannelAuthenticationRequestStore : IBackChannelAuthenticationRequestStore
    {
        private readonly DataContext _context;
        private readonly ILogger<BackChannelAuthenticationRequestStore> _logger;
        private readonly IHttpContextAccessor _contextAccessor;

        public BackChannelAuthenticationRequestStore(DataContext context, ILogger<BackChannelAuthenticationRequestStore> logger, IHttpContextAccessor contextAccessor)
        {
            _context = context;
            _logger = logger;
            _contextAccessor = contextAccessor;
        }

        public async Task<string> CreateRequestAsync(BackChannelAuthenticationRequest request)
        {
            
            var cibaLoginRequest = new Fido2me.Data.OIDC.ciba.CibaLoginRequest()
            {
                Id = Guid.NewGuid().ToString("N"),
                RequestId = Guid.NewGuid().ToString("N"),
                BindingMessage = request.BindingMessage,
                IdP = request.IdP,
                SubjectId = request.Subject.FindFirst(c => c.Type == "sub").Value,
                Tenant = request.Tenant, 
                RequestedScopes = request.RequestedScopes.ToArray(),
                AuthorizedScopes = null,
                ClientId = request.ClientId,
                CreatedAt = DateTimeOffset.Now,
                Lifetime = request.Lifetime,  
            };
            var r = await _context.CibaLoginRequests.AddAsync(cibaLoginRequest);
            await _context.SaveChangesAsync();

            return cibaLoginRequest.RequestId;
        }

        public async Task<BackChannelAuthenticationRequest> GetByAuthenticationRequestIdAsync(string requestId)
        {
            var loginRequest = await _context.CibaLoginRequests
            .AsNoTracking()
            .Where(c => c.RequestId == requestId)
            .Select(c => new BackChannelAuthenticationRequest()
            {               
               BindingMessage = c.BindingMessage,
               InternalId = c.Id,
               Subject = IdentityServerHelper.GenerateSubjectById(c.SubjectId),
               ClientId = c.ClientId,
               RequestedScopes = c.RequestedScopes,
               AuthorizedScopes = c.AuthorizedScopes,
               CreationTime = c.CreatedAt.UtcDateTime,
               Lifetime = c.Lifetime,
               IsComplete = c.IsComplete,

            }).FirstOrDefaultAsync();

            return loginRequest;
        }

        public async Task<BackChannelAuthenticationRequest> GetByInternalIdAsync(string id)
        {
            var loginRequest = await _context.CibaLoginRequests
                .AsNoTracking()
               .Where(c => c.Id == id)
               .Select(c => new BackChannelAuthenticationRequest()
               {
                   BindingMessage = c.BindingMessage,
                   InternalId = c.Id,
                   Subject = IdentityServerHelper.GenerateSubjectById(c.SubjectId),
                   ClientId = c.ClientId,
                   RequestedScopes = c.RequestedScopes,
                   AuthorizedScopes = c.AuthorizedScopes,
                   CreationTime = c.CreatedAt.UtcDateTime,
                   Lifetime = c.Lifetime,
                   IsComplete = c.IsComplete,

               }).FirstOrDefaultAsync();
               
            return loginRequest;
        }

        public async Task<IEnumerable<BackChannelAuthenticationRequest>> GetLoginsForUserAsync(string subjectId, string clientId = null)
        {
            var subject = IdentityServerHelper.GenerateSubjectById(subjectId);

            var loginRequests = await _context.CibaLoginRequests
                .Where(c => c.SubjectId == subjectId)
                .Select(c => new BackChannelAuthenticationRequest() 
                {
                    BindingMessage = c.BindingMessage,
                    InternalId = c.Id,
                    Subject = subject,
                    ClientId = clientId, 
                    RequestedScopes = c.RequestedScopes,
                    AuthorizedScopes = c.AuthorizedScopes,
                    CreationTime = c.CreatedAt.UtcDateTime,
                    Lifetime= c.Lifetime,
                    IsComplete = c.IsComplete,
                })
                .ToListAsync();

            return loginRequests;
        }

        public async Task RemoveByInternalIdAsync(string id)
        {
            // do not remove for now

            //var cibaRequest = await _context.CibaLoginRequests.FirstOrDefaultAsync(c => c.Id == id);
            //_context.CibaLoginRequests.Remove(cibaRequest);
            //await _context.SaveChangesAsync();
        }

        public async Task UpdateByInternalIdAsync(string id, BackChannelAuthenticationRequest request)
        {
            var loginRequest = await _context.CibaLoginRequests
                .Where(c => c.Id == id)
                .FirstOrDefaultAsync();

            // CIBA updates
            loginRequest.IsComplete = request.IsComplete;
            loginRequest.AuthorizedScopes = request.AuthorizedScopes.ToArray();

            // FIDO updates
            // provide username, used credentialId
            var username = _contextAccessor.HttpContext.User.FindFirst(c => c.Type == "name").Value;
            var credId = _contextAccessor.HttpContext.User.FindFirst(c => c.Type == "credId").Value;
            loginRequest.Username = username;
            loginRequest.CredentialId = credId;

            await _context.SaveChangesAsync();

            return;
        }
    }
}
