using Duende.IdentityServer;
using Fido2me.Data;
using Fido2me.Data.FIDO2;
using Fido2me.Services;
using Fido2NetLib;
using Fido2NetLib.Development;
using Fido2NetLib.Objects;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;
using static Fido2NetLib.Fido2;

namespace Fido2me.Controllers
{
    public class Fido2Controller : Controller 
    {
        private IFido2 _fido2;
        public static IMetadataService _mds;
        public static readonly DevelopmentInMemoryStore DemoStorage = new DevelopmentInMemoryStore();
        private readonly IDataProtector _protector;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IFidoService _fidoService;

        private readonly DataContext _context;

        public Fido2Controller(DataContext context, IFido2 fido2, IMetadataService metadataService, IDataProtectionProvider provider, IFidoService fidoService, IHttpContextAccessor contextAccessor)
        {
            _context = context;
            _fido2 = fido2;
            _mds = metadataService;
            _protector = provider.CreateProtector("fido2");
            _fidoService = fidoService;
        }

        private string FormatException(Exception e)
        {
            return string.Format("{0}{1}", e.Message, e.InnerException != null ? " (" + e.InnerException.Message + ")" : "");
        }

        [HttpPost]        
        [Route("/account/register/start")]
        public JsonResult AccountRegisterStart([FromForm] string displayName, [FromForm] string authType)
        {
            try
            {
                
                if (string.IsNullOrWhiteSpace(displayName))
                {
                    displayName = $"Fido2me created at {DateTime.UtcNow}";
                    
                }
                var username = displayName.Trim();

                // 1. Create a temporary user to store in an encrypted cookie
                var user = new Fido2User
                {
                    DisplayName = displayName + DateTime.Now.ToString(),
                    Name = username,
                    Id = Guid.NewGuid().ToByteArray() // byte representation of userID is required
                };

                // 2. Get user existing keys by username
                // var existingKeys = DemoStorage.GetCredentialsByUser(user).Select(c => c.Descriptor).ToList();
                // it's a new registration, we will try to add more credentials or merge accounts later, RP will allow to create multiple accounts on the same single authenticator
                var existingKeys = new List<PublicKeyCredentialDescriptor>();

                // 3. Create options
                var authenticatorSelection = new AuthenticatorSelection
                {
                    RequireResidentKey = true,
                    UserVerification = UserVerificationRequirement.Required,
                };

                if (!string.IsNullOrEmpty(authType))
                    authenticatorSelection.AuthenticatorAttachment = authType.ToEnum<AuthenticatorAttachment>();

                var exts = new AuthenticationExtensionsClientInputs()
                {
                    Extensions = true,
                    UserVerificationMethod = true,
                };

                var options = _fido2.RequestNewCredential(user, existingKeys, authenticatorSelection, AttestationConveyancePreference.Direct, exts);

                // 4. Temporarily store options, session/in-memory cache/redis/db
                //HttpContext.Session.SetString("fido2.attestationOptions", options.ToJson());
                string protectedPayload = _protector.Protect(options.ToJson());
                HttpContext.Response.Cookies.Append("registration", protectedPayload, new CookieOptions { HttpOnly = true, IsEssential = true, Secure = true, Expires = DateTime.Now.AddMinutes(5) });

                // 5. return options to client
                return Json(options);
            }
            catch (Exception e)
            {
                return Json(new CredentialCreateOptions { Status = "error", ErrorMessage = FormatException(e) });
            }
        }

        [HttpPost]
        [Route("/account/register/complete")]
        public async Task<IActionResult> AccountRegisterComplete([FromBody] AuthenticatorAttestationRawResponse attestationResponse, CancellationToken cancellationToken)
        {
            try
            {
                // 1. get the options we sent the client                
                var protectedOptions = HttpContext.Request.Cookies["registration"];
                if (protectedOptions == null)
                {
                    return BadRequest(new { message = "bad request" });
                }
                var unprotectedOptions = _protector.Unprotect(protectedOptions);

                var options = CredentialCreateOptions.FromJson(unprotectedOptions);
                HttpContext.Response.Cookies.Delete("registration");
                
                // 2. Create callback so that lib can verify credential id is unique to this user
                // GUID will be unique

                // 2. Verify and make the credentials
                var attestation = await _fido2.MakeNewCredentialAsync(attestationResponse, options, null, lazyAttestation: true, cancellationToken: cancellationToken);
                if (attestation.Result.AttestationVerificationStatus == AttestationVerificationStatus.Failed)
                    return Json(attestation);
     
                await _fidoService.CompleteAttestation(attestation.Result);

                // 4. return "ok" to the client
                return Json(attestation);
            }
            catch (Exception e)
            {
                return Json(new CredentialMakeResult(status: "error", errorMessage: FormatException(e), result: null));
            }
        }

        [HttpPost]
        [Route("/account/login/start")]
        public ActionResult AccountLoginStart([FromBody] string username)
        {
            try
            {

                var exts = new AuthenticationExtensionsClientInputs()
                {
                    UserVerificationMethod = true
                };

                // 3. Create options
                var options = _fido2.GetAssertionOptions(
                    allowedCredentials: new List<PublicKeyCredentialDescriptor>(),
                    UserVerificationRequirement.Required,
                    exts
                );

                // 4. Temporarily store options, session/in-memory cache/redis/db
                string protectedPayload = _protector.Protect(options.ToJson());
                HttpContext.Response.Cookies.Append("login", protectedPayload, new CookieOptions { HttpOnly = true, IsEssential = true, Secure = true, Expires = DateTime.Now.AddMinutes(5) });

                // 5. Return options to client
                return Json(options);
            }

            catch (Exception e)
            {
                return Json(new AssertionOptions { Status = "error", ErrorMessage = FormatException(e) });
            }
        }

        [HttpPost]
        [Route("/account/login/complete")]
        public async Task<IActionResult> AccountLoginComplete([FromBody] AuthenticatorAssertionRawResponse clientResponse, CancellationToken cancellationToken)
        {
            try
            {
                // 1. Get the assertion options we sent the client
                var protectedOptions = HttpContext.Request.Cookies["login"];
                if (protectedOptions == null)
                {
                    return BadRequest(new { message = "bad request" });
                }
                var unprotectedOptions = _protector.Unprotect(protectedOptions);

                var options = AssertionOptions.FromJson(unprotectedOptions);
                HttpContext.Response.Cookies.Delete("login");

                // 2. Get registered credential from database
                var credential = await _fidoService.GetCredentialAsync(clientResponse.Id);                
                
                // 3. Get credential counter from database
                var storedCounter = credential.SignatureCounter;

                // 4. Create callback to check if userhandle owns the credentialId
                // Check Raw.Id == UserHandle?

                IsUserHandleOwnerOfCredentialIdAsync callback = static async (args, cancellationToken) =>
                {
                    var storedCreds = await DemoStorage.GetCredentialsByUserHandleAsync(args.UserHandle, cancellationToken);
                    return storedCreds.Exists(c => c.Descriptor.Id.SequenceEqual(args.CredentialId));
                };

                // 5. Make the assertion
                var res = await _fido2.MakeAssertionAsync(clientResponse, options, credential.PublicKey, storedCounter, null, cancellationToken: cancellationToken);

                // 6. Store the updated counter
                credential.SignatureCounter = res.Counter;
                await _fidoService.UpdateCredentialAsync(credential);
                //DemoStorage.UpdateCounter(res.CredentialId, res.Counter);

                var user = new IdentityServerUser(credential.AccountId.ToString())
                {
                    DisplayName = credential.AaGuid.ToString()
                };
                await HttpContext.SignInAsync(user);

                // 7. return OK to client
                return Json(res);
            }
            catch (Exception e)
            {
                return Json(new AssertionVerificationResult { Status = "error", ErrorMessage = FormatException(e) });
            }
        }
    }
}
