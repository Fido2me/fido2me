using System.Security.Claims;

namespace Fido2me.Helpers
{
    public static class IdentityServerHelper
    {
        public static ClaimsPrincipal GenerateSubjectById(string subjectId)
        {
            var claims = new Claim[] {
                    new Claim("sub", subjectId),
                    new Claim("auth_time", DateTime.UtcNow.Ticks.ToString()),
                    new Claim("idp", "fido2me"),
                };
            var identity = new ClaimsIdentity(claims, "ciba");
            var subject = new ClaimsPrincipal(identity);
            return subject;
        }
    }
}
