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
                InternalId = Guid.NewGuid().ToString("N"),
                BindingMessage = request.BindingMessage,
                IdP = request.IdP,
                SubjectId = request.Subject.FindFirst(c => c.Type == "sub").Value,
                Tenant = request.Tenant, 
                RequestedScopes = request.RequestedScopes.ToArray(),
                AuthorizedScopes = null,
            };
            var r = await _context.CibaLoginRequests.AddAsync(cibaLoginRequest);
            await _context.SaveChangesAsync();

            return "success?";
        }

        public async Task<BackChannelAuthenticationRequest> GetByAuthenticationRequestIdAsync(string requestId)
        {
            return null;
            //var loginRequest = await _context.CibaLoginRequests
            //    .AsNoTracking()
            //    .Where(c => c.)
        }

        public async Task<BackChannelAuthenticationRequest> GetByInternalIdAsync(string id)
        {
            var loginRequest = await _context.CibaLoginRequests
                .AsNoTracking()
               .Where(c => c.InternalId == id)
               .Select(c => new BackChannelAuthenticationRequest()
               {
                   BindingMessage = c.BindingMessage,
                   InternalId = c.InternalId,
                   Subject = IdentityServerHelper.GenerateSubjectById(c.SubjectId), // if no entries, our subject doesn't matter, so reuse the parameter
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
                    InternalId = c.InternalId,
                    Subject = subject, // if no entries, our subject doesn't matter, so reuse the parameter
                    ClientId = clientId, // from db?
                    RequestedScopes = c.RequestedScopes,
                    AuthorizedScopes = c.AuthorizedScopes,
                })
                .ToListAsync();

            return loginRequests;
        }

        public Task RemoveByInternalIdAsync(string id)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateByInternalIdAsync(string id, BackChannelAuthenticationRequest request)
        {
            var loginRequest = await _context.CibaLoginRequests
                .Where(c => c.InternalId == id)
                .FirstOrDefaultAsync();

            loginRequest.IsComplete = request.IsComplete;
            loginRequest.AuthorizedScopes = request.AuthorizedScopes.ToArray();
            await _context.SaveChangesAsync();

            return;
        }
    }
}
