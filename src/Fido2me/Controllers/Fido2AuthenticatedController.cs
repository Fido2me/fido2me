using Fido2me.Data;
using Fido2me.Data.FIDO2;
using Fido2me.Services;
using Fido2NetLib;
using Fido2NetLib.Development;
using Fido2NetLib.Objects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;
using static Fido2NetLib.Fido2;

namespace Fido2me.Controllers
{
    [Authorize]
    public class Fido2AuthenticatedController : Controller 
    {
        private IFido2 _fido2;
        public static IMetadataService _mds;
        private readonly IDataProtector _protector;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IFidoService _registrationService;

        private readonly DataContext _context;

        public Fido2AuthenticatedController(DataContext context, IFido2 fido2, IMetadataService metadataService, IDataProtectionProvider provider, IFidoService registrationService, IHttpContextAccessor contextAccessor)
        {
            _context = context;
            _fido2 = fido2;
            _mds = metadataService;
            _protector = provider.CreateProtector("fido2");
            _registrationService = registrationService;
        }

        private string FormatException(Exception e)
        {
            return string.Format("{0}{1}", e.Message, e.InnerException != null ? " (" + e.InnerException.Message + ")" : "");
        }

        [HttpPost]
        [Route("/account/add/start")]
        public IActionResult AccountAddStart([FromForm] string displayName, [FromForm] string authType)
        {
            // this will be similar to creating a new account but we will use existing user.id
            if (!User.Identity.IsAuthenticated)
            {
                // should be controlled by controller only?
                return BadRequest("User is not authenticated");
            }
            try
            {
                // get the current account and use User.Id from it
                // initiate registration process - another account should exist
                var userSomething = User.Identity.Name;
                // TODO: Replace with authenticated user id
                var userId = Guid.NewGuid().ToByteArray();

                if (string.IsNullOrWhiteSpace(displayName))
                {
                    displayName = $"Fido2me created at {DateTime.UtcNow}";
                    
                }
                var username = displayName.Trim();

                // 1. Create a temporary user to store in an encrypted cookie
                var user = new Fido2User
                {
                    DisplayName = displayName,
                    Name = username,
                    Id = userId
                };

                // allow any key to be registred
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

                string protectedPayload = _protector.Protect(options.ToJson());
                HttpContext.Response.Cookies.Append("add", protectedPayload, new CookieOptions { HttpOnly = true, IsEssential = true, Secure = true, Expires = DateTime.Now.AddMinutes(5) });

                // 5. return options to client
                return Json(options);
            }
            catch (Exception e)
            {
                return Json(new CredentialCreateOptions { Status = "error", ErrorMessage = FormatException(e) });
            }
        }

        [HttpPost]
        [Route("/account/add/complete")]
        public async Task<IActionResult> AccountAddComplete([FromBody] AuthenticatorAttestationRawResponse attestationResponse, CancellationToken cancellationToken)
        {
            try
            {
                // 1. get the options we sent the client                
                var protectedOptions = HttpContext.Request.Cookies["add"];
                if (protectedOptions == null)
                {
                    return BadRequest(new { message = "bad request" });
                }
                var unprotectedOptions = _protector.Unprotect(protectedOptions);

                var options = CredentialCreateOptions.FromJson(unprotectedOptions);
                HttpContext.Response.Cookies.Delete("add");

                // 2. Verify and make the credentials
                var success = await _fido2.MakeNewCredentialAsync(attestationResponse, options, null, cancellationToken: cancellationToken);

                // 3. Store the credentials in db
                var credential = new Credential()
                {
                    Id = success.Result.CredentialId,
                    AaGuid = success.Result.Aaguid,                   
                    CredType = success.Result.CredType,
                    PublicKey = success.Result.PublicKey,
                    UserHandle = success.Result.User.Id,
                    RegDate = DateTimeOffset.Now,
                    SignatureCounter = success.Result.Counter,
                    AccountId = new Guid(success.Result.User.Id)
                };
                await _registrationService.AddCredentialAsync(credential);

                // 4. return "ok" to the client
                return Json(success);
            }
            catch (Exception e)
            {
                return Json(new CredentialMakeResult(status: "error", errorMessage: FormatException(e), result: null));
            }
        }

        [HttpPost]
        [Route("/account/merge/start")]
        public ActionResult AccountMergeStart([FromForm] string authType)
        {
            try
            {
                var authenticatorSelection = new AuthenticatorSelection
                {
                    RequireResidentKey = true,
                    UserVerification = UserVerificationRequirement.Required,
                };

                if (!string.IsNullOrEmpty(authType))
                    authenticatorSelection.AuthenticatorAttachment = authType.ToEnum<AuthenticatorAttachment>();

                var exts = new AuthenticationExtensionsClientInputs()
                {
                    UserVerificationMethod = true,             
                };

                // 3. Create options
                var options = _fido2.GetAssertionOptions(
                    allowedCredentials: new List<PublicKeyCredentialDescriptor>(),
                    UserVerificationRequirement.Required,
                    exts
                );

                // 4. Temporarily store options, session/in-memory cache/redis/db
                string protectedPayload = _protector.Protect(options.ToJson());
                HttpContext.Response.Cookies.Append("merge", protectedPayload, new CookieOptions { HttpOnly = true, IsEssential = true, Secure = true, Expires = DateTime.Now.AddMinutes(5) });

                // 5. Return options to client
                return Json(options);
            }

            catch (Exception e)
            {
                return Json(new AssertionOptions { Status = "error", ErrorMessage = FormatException(e) });
            }
        }

        [HttpPost]
        [Route("/account/merge/complete")]
        public async Task<IActionResult> AccountMergeComplete([FromBody] AuthenticatorAssertionRawResponse clientResponse, CancellationToken cancellationToken)
        {
            try
            {
                // 1. Get the assertion options we sent the client
                var protectedOptions = HttpContext.Request.Cookies["merge"];
                if (protectedOptions == null)
                {
                    return BadRequest(new { message = "bad request" });
                }
                var unprotectedOptions = _protector.Unprotect(protectedOptions);

                var options = AssertionOptions.FromJson(unprotectedOptions);
                HttpContext.Response.Cookies.Delete("merge");

                // 2. Get registered credential from database
                var credential = await _registrationService.GetCredentialAsync(clientResponse.Id);
                if (credential == null)
                {
                    return NotFound();
                }

                // 3. Get credential counter from database
                var storedCounter = credential.SignatureCounter;

                // 4. Create callback to check if userhandle owns the credentialId
                // Check Raw.Id == UserHandle?

                /*
                IsUserHandleOwnerOfCredentialIdAsync callback = static async (args, cancellationToken) =>
                {
                    var storedCreds = await DemoStorage.GetCredentialsByUserHandleAsync(args.UserHandle, cancellationToken);
                    return storedCreds.Exists(c => c.Descriptor.Id.SequenceEqual(args.CredentialId));
                };
                */

                // 5. Make the assertion
                var res = await _fido2.MakeAssertionAsync(clientResponse, options, credential.PublicKey, storedCounter, null, cancellationToken: cancellationToken);

                // 6. Store the updated counter
                credential.SignatureCounter = res.Counter;
                await _registrationService.UpdateCredentialAsync(credential);
                //DemoStorage.UpdateCounter(res.CredentialId, res.Counter);

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
