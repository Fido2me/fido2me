using System.Security.Claims;

namespace Fido2me.Helpers
{
    public static class IdentityServerHelper
    {
        public static ClaimsPrincipal GenerateSubjectById(string subjectId)
        {
            var claims = new Claim[] {
                    new Claim("sub", subjectId)
                };
            var identity = new ClaimsIdentity(claims, "ciba");
            var subject = new ClaimsPrincipal(identity);
            return subject;
        }
    }
}
