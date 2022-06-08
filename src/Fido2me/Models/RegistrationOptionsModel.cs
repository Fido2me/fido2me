using Fido2NetLib;

namespace Fido2me.Models
{
    public class RegistrationOptionsModel
    {
        private CredentialCreateOptions options;

        public RegistrationOptionsModel(CredentialCreateOptions options)
        {
            this.options = options;
        }
    }
}
