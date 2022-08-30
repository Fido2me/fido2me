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

        public BackChannelAuthenticationRequestStore(DataContext context, ILogger<BackChannelAuthenticationRequestStore> logger)
        {
            _context = context;
            _logger = logger;
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
            };
            var r = await _context.CibaLoginRequests.AddAsync(cibaLoginRequest);
            await _context.SaveChangesAsync();

            return cibaLoginRequest.RequestId;
        }

        public async Task<BackChannelAuthenticationRequest> GetByAuthenticationRequestIdAsync(string requestId)
        {
            var loginRequest = await _context.CibaLoginRequests
            .AsNoTracking()
            .Where(c => c.Id == requestId)
            .Select(c => new BackChannelAuthenticationRequest()
            {               
               BindingMessage = c.BindingMessage,
               InternalId = c.Id,
               Subject = IdentityServerHelper.GenerateSubjectById(c.SubjectId),
               ClientId = "", // from db?
               RequestedScopes = c.RequestedScopes,
               AuthorizedScopes = c.AuthorizedScopes,

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
                   ClientId = "", // from db?
                   RequestedScopes = c.RequestedScopes,
                   AuthorizedScopes = c.AuthorizedScopes,


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
                    Subject = subject, // if no entries, our subject doesn't matter, so reuse the parameter
                    ClientId = clientId, // from db?
                    RequestedScopes = c.RequestedScopes,
                    AuthorizedScopes = c.AuthorizedScopes,
                })
                .ToListAsync();

            return loginRequests;
        }

        public async Task RemoveByInternalIdAsync(string id)
        {
            var cibaRequest = await _context.CibaLoginRequests.FirstOrDefaultAsync(c => c.Id == id);
            _context.CibaLoginRequests.Remove(cibaRequest);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateByInternalIdAsync(string id, BackChannelAuthenticationRequest request)
        {
            var loginRequest = await _context.CibaLoginRequests
                .Where(c => c.Id == id)
                .FirstOrDefaultAsync();

            loginRequest.IsComplete = request.IsComplete;
            loginRequest.AuthorizedScopes = request.AuthorizedScopes.ToArray();
            await _context.SaveChangesAsync();

            return;
        }
    }
}
