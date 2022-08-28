using Duende.IdentityServer.Models;
using Duende.IdentityServer.Validation;
using System.ComponentModel.DataAnnotations;

namespace Fido2me.Data.OIDC.ciba
{
    public class CibaLoginRequest
    {
        [Key]
        public string InternalId { get; set; }

        /// <summary>
        /// Gets or sets the subject.
        /// </summary>
        public string SubjectId { get; set; }

        /// <summary>
        /// Gets or sets the binding message.
        /// </summary>
        public string BindingMessage { get; set; }

        /// <summary>
        /// Gets or sets the authentication context reference classes.
        /// </summary>
        //public IEnumerable<string> AuthenticationContextReferenceClasses { get; set; }

        /// <summary>
        /// Gets or sets the tenant passed in the acr_values.
        /// </summary>
        public string Tenant { get; set; }

        /// <summary>
        /// Gets or sets the idp passed in the acr_values.
        /// </summary>
        public string IdP { get; set; }

        public string[] RequestedScopes { get; set; }

        public string[] AuthorizedScopes { get; set; }

        public bool IsComplete { get; set; }

        /// <summary>
        /// Gets or sets the resource indicator.
        /// </summary>
        //public IEnumerable<string> RequestedResourceIndicators { get; set; }

        /// <summary>
        /// Gets or sets the client.
        /// </summary>
        //public Client Client { get; set; }

        /// <summary>
        /// Gets or sets the validated resources.
        /// </summary>
        //public ResourceValidationResult ValidatedResources { get; set; }
    }
}
