﻿using System.Text.Json.Serialization;

namespace Fido2NetLib.Objects
{
    /// <summary>
    /// Holds parsed credential data
    /// </summary>
    public class BaseAttestationVerification : AssertionVerificationResult
    {

        [JsonConverter(typeof(Base64UrlConverter))]
        public byte[] PublicKey { get; set; }

        public Fido2User User { get; set; }
        public string CredType { get; set; }
        public System.Guid Aaguid { get; set; }

        public AttestationVerificationStatus AttestationVerificationStatus { get; set; }
    }
}
