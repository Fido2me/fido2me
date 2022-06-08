﻿using System;
using System.Security.Cryptography.X509Certificates;

using Fido2NetLib;

using Xunit;

namespace Test
{
    public class CryptoUtilsTests
    {
        [Fact]
        public void TestCertInCRLFalseCase()
        {
            byte[] certBytes = Convert.FromBase64String("MIIDAzCCAqigAwIBAgIPBFTYzwOQmHjntsvY0AGOMAoGCCqGSM49BAMCMG8xCzAJBgNVBAYTAlVTMRYwFAYDVQQKDA1GSURPIEFsbGlhbmNlMS8wLQYDVQQLDCZGQUtFIE1ldGFkYXRhIDMgQkxPQiBJTlRFUk1FRElBVEUgRkFLRTEXMBUGA1UEAwwORkFLRSBDQS0xIEZBS0UwHhcNMTcwMjAxMDAwMDAwWhcNMzAwMTMxMjM1OTU5WjCBjjELMAkGA1UEBhMCVVMxFjAUBgNVBAoMDUZJRE8gQWxsaWFuY2UxMjAwBgNVBAsMKUZBS0UgTWV0YWRhdGEgMyBCTE9CIFNpZ25pbmcgU2lnbmluZyBGQUtFMTMwMQYDVQQDDCpGQUtFIE1ldGFkYXRhIDMgQkxPQiBTaWduaW5nIFNpZ25lciA0IEZBS0UwWTATBgcqhkjOPQIBBggqhkjOPQMBBwNCAATL3eRNA9YIQ3mAsHfcO3x0rHxqg3xkQUb2E4Mo39L6SLXnz82D5Nnq+59Ah1hNfL5OEtxdgy+/kIJyiScl4+T8o4IBBTCCAQEwCwYDVR0PBAQDAgbAMAwGA1UdEwEB/wQCMAAwHQYDVR0OBBYEFPl4RxJ2M8prAEvqnSFK4+3nN8SqMB8GA1UdIwQYMBaAFKOEp6Rkook8Cr8XnqIN8BIaptfLMEgGA1UdHwRBMD8wPaA7oDmGN2h0dHBzOi8vbWRzMy5jZXJ0aW5mcmEuZmlkb2FsbGlhbmNlLm9yZy9jcmwvTURTQ0EtMS5jcmwwWgYDVR0gBFMwUTBPBgsrBgEEAYLlHAEDATBAMD4GCCsGAQUFBwIBFjJodHRwczovL21kczMuY2VydGluZnJhLmZpZG9hbGxpYW5jZS5vcmcvcmVwb3NpdG9yeTAKBggqhkjOPQQDAgNJADBGAiEAxIq00OoEowGSIlqPzVQtqKTgCJpqSHu3NYZHgQIIbKICIQCZYm9Z0KnEhzWIc0bwa0sLfZ/AMJ8vhM5B1jrz8mgmBA==");

            byte[] crl = Convert.FromBase64String("MIIB7DCCAZICAQEwCgYIKoZIzj0EAwIwbzELMAkGA1UEBhMCVVMxFjAUBgNVBAoMDUZJRE8gQWxsaWFuY2UxLzAtBgNVBAsMJkZBS0UgTWV0YWRhdGEgMyBCTE9CIElOVEVSTUVESUFURSBGQUtFMRcwFQYDVQQDDA5GQUtFIENBLTEgRkFLRRcNMTgwMjAxMDAwMDAwWhcNMjIwMjAxMDAwMDAwWjCBwDAuAg8ELS9CzLtxNJTOFTHXiV8XDTE2MDQxMzAwMDAwMFowDDAKBgNVHRUEAwoBADAuAg8ExejzukpclaXnFLGvxDEXDTE3MDMyNTAwMDAwMFowDDAKBgNVHRUEAwoBADAuAg8Er13ouX8KNf3VOr4OzQEXDTE2MDMwMTAwMDAwMFowDDAKBgNVHRUEAwoBADAuAg8EgGdJ3jB7vVF1om1z9fMXDTE4MDMyNTAwMDAwMFowDDAKBgNVHRUEAwoBAKAvMC0wCgYDVR0UBAMCAQEwHwYDVR0jBBgwFoAUo4SnpGSiiTwKvxeeog3wEhqm18swCgYIKoZIzj0EAwIDSAAwRQIgDgtshLf5/82mHcOgl2TsUizHsjLCslmQVDdSPcolS8UCIQDa5MSjQbX1v8MkCPpzxbrBb1I510aSTuZB0RUuwPnOYw==");

            var cert = new X509Certificate2(certBytes);

            Assert.False(CryptoUtils.IsCertInCRL(crl, cert));
        }

        [Fact]
        public void TestCertInCRLTrueCase()
        {
            byte[] certBytes = Convert.FromBase64String("MIIDAjCCAqigAwIBAgIPBIBnSd4we71RdaJtc / XzMAoGCCqGSM49BAMCMG8xCzAJBgNVBAYTAlVTMRYwFAYDVQQKDA1GSURPIEFsbGlhbmNlMS8wLQYDVQQLDCZGQUtFIE1ldGFkYXRhIDMgQkxPQiBJTlRFUk1FRElBVEUgRkFLRTEXMBUGA1UEAwwORkFLRSBDQS0xIEZBS0UwHhcNMTcwMjAxMDAwMDAwWhcNMzAwMTMxMjM1OTU5WjCBjjELMAkGA1UEBhMCVVMxFjAUBgNVBAoMDUZJRE8gQWxsaWFuY2UxMjAwBgNVBAsMKUZBS0UgTWV0YWRhdGEgMyBCTE9CIFNpZ25pbmcgU2lnbmluZyBGQUtFMTMwMQYDVQQDDCpGQUtFIE1ldGFkYXRhIDMgQkxPQiBTaWduaW5nIFNpZ25lciA0IEZBS0UwWTATBgcqhkjOPQIBBggqhkjOPQMBBwNCAATL3eRNA9YIQ3mAsHfcO3x0rHxqg3xkQUb2E4Mo39L6SLXnz82D5Nnq + 59Ah1hNfL5OEtxdgy +/ kIJyiScl4 + T8o4IBBTCCAQEwCwYDVR0PBAQDAgbAMAwGA1UdEwEB / wQCMAAwHQYDVR0OBBYEFPl4RxJ2M8prAEvqnSFK4 + 3nN8SqMB8GA1UdIwQYMBaAFKOEp6Rkook8Cr8XnqIN8BIaptfLMEgGA1UdHwRBMD8wPaA7oDmGN2h0dHBzOi8vbWRzMy5jZXJ0aW5mcmEuZmlkb2FsbGlhbmNlLm9yZy9jcmwvTURTQ0EtMS5jcmwwWgYDVR0gBFMwUTBPBgsrBgEEAYLlHAEDATBAMD4GCCsGAQUFBwIBFjJodHRwczovL21kczMuY2VydGluZnJhLmZpZG9hbGxpYW5jZS5vcmcvcmVwb3NpdG9yeTAKBggqhkjOPQQDAgNIADBFAiB3yVejfuPNQT + 5VPY5gDcPXAdwA9Pudwe1M0BcGsa5 + gIhAJ0opi4Y / w26gNaAvsCvalwCqI6QYQCP1bjGSMgu3K1e");

            byte[] crl = Convert.FromBase64String("MIIB7DCCAZICAQEwCgYIKoZIzj0EAwIwbzELMAkGA1UEBhMCVVMxFjAUBgNVBAoMDUZJRE8gQWxsaWFuY2UxLzAtBgNVBAsMJkZBS0UgTWV0YWRhdGEgMyBCTE9CIElOVEVSTUVESUFURSBGQUtFMRcwFQYDVQQDDA5GQUtFIENBLTEgRkFLRRcNMTgwMjAxMDAwMDAwWhcNMjIwMjAxMDAwMDAwWjCBwDAuAg8ELS9CzLtxNJTOFTHXiV8XDTE2MDQxMzAwMDAwMFowDDAKBgNVHRUEAwoBADAuAg8ExejzukpclaXnFLGvxDEXDTE3MDMyNTAwMDAwMFowDDAKBgNVHRUEAwoBADAuAg8Er13ouX8KNf3VOr4OzQEXDTE2MDMwMTAwMDAwMFowDDAKBgNVHRUEAwoBADAuAg8EgGdJ3jB7vVF1om1z9fMXDTE4MDMyNTAwMDAwMFowDDAKBgNVHRUEAwoBAKAvMC0wCgYDVR0UBAMCAQEwHwYDVR0jBBgwFoAUo4SnpGSiiTwKvxeeog3wEhqm18swCgYIKoZIzj0EAwIDSAAwRQIgDgtshLf5/82mHcOgl2TsUizHsjLCslmQVDdSPcolS8UCIQDa5MSjQbX1v8MkCPpzxbrBb1I510aSTuZB0RUuwPnOYw==");

            var cert = new X509Certificate2(certBytes);

            Assert.True(CryptoUtils.IsCertInCRL(crl, cert));
        }

        [Fact]
        public void TestValidateTrustChainRootAnchor()
        {
            var attestationRootCertificates = new X509Certificate2[3] 
            { 
                new X509Certificate2(Convert.FromBase64String("MIIBfjCCASWgAwIBAgIBATAKBggqhkjOPQQDAjAXMRUwEwYDVQQDDAxGVCBGSURPIDAyMDAwIBcNMTYwNTAxMDAwMDAwWhgPMjA1MDA1MDEwMDAwMDBaMBcxFTATBgNVBAMMDEZUIEZJRE8gMDIwMDBZMBMGByqGSM49AgEGCCqGSM49AwEHA0IABNBmrRqVOxztTJVN19vtdqcL7tKQeol2nnM2/yYgvksZnr50SKbVgIEkzHQVOu80LVEE3lVheO1HjggxAlT6o4WjYDBeMB0GA1UdDgQWBBRJFWQt1bvG3jM6XgmV/IcjNtO/CzAfBgNVHSMEGDAWgBRJFWQt1bvG3jM6XgmV/IcjNtO/CzAMBgNVHRMEBTADAQH/MA4GA1UdDwEB/wQEAwIBBjAKBggqhkjOPQQDAgNHADBEAiAwfPqgIWIUB+QBBaVGsdHy0s5RMxlkzpSX/zSyTZmUpQIgB2wJ6nZRM8oX/nA43Rh6SJovM2XwCCH//+LirBAbB0M=")),
                new X509Certificate2(Convert.FromBase64String("MIIB2DCCAX6gAwIBAgIQFZ97ws2JGPEoa5NI+p8z1jAKBggqhkjOPQQDAjBLMQswCQYDVQQGEwJDTjEdMBsGA1UECgwURmVpdGlhbiBUZWNobm9sb2dpZXMxHTAbBgNVBAMMFEZlaXRpYW4gRklETyBSb290IENBMCAXDTE4MDQwMTAwMDAwMFoYDzIwNDgwMzMxMjM1OTU5WjBLMQswCQYDVQQGEwJDTjEdMBsGA1UECgwURmVpdGlhbiBUZWNobm9sb2dpZXMxHTAbBgNVBAMMFEZlaXRpYW4gRklETyBSb290IENBMFkwEwYHKoZIzj0CAQYIKoZIzj0DAQcDQgAEnfAKbjvMX1Ey1b6k+WQQdNVMt9JgGWyJ3PvM4BSK5XqTfo++0oAj/4tnwyIL0HFBR9St+ktjqSXDfjiXAurs86NCMEAwHQYDVR0OBBYEFNGhmE2Bf8O5a/YHZ71QEv6QRfFUMA8GA1UdEwEB/wQFMAMBAf8wDgYDVR0PAQH/BAQDAgEGMAoGCCqGSM49BAMCA0gAMEUCIQC3sT1lBjGeF+xKTpzV1KYU2ckahTd4mLJyzYOhaHv4igIgD2JYkfyH5Q4Bpo8rroO0It7oYjF2kgy/eSZ3U9Glaqw=")),
                new X509Certificate2(Convert.FromBase64String("MIIB2DCCAX6gAwIBAgIQGBUrQbdDrm20FZnDsX2CBTAKBggqhkjOPQQDAjBLMQswCQYDVQQGEwJVUzEdMBsGA1UECgwURmVpdGlhbiBUZWNobm9sb2dpZXMxHTAbBgNVBAMMFEZlaXRpYW4gRklETyBSb290IENBMCAXDTE4MDQwMTAwMDAwMFoYDzIwNDgwMzMxMjM1OTU5WjBLMQswCQYDVQQGEwJVUzEdMBsGA1UECgwURmVpdGlhbiBUZWNobm9sb2dpZXMxHTAbBgNVBAMMFEZlaXRpYW4gRklETyBSb290IENBMFkwEwYHKoZIzj0CAQYIKoZIzj0DAQcDQgAEsFYEEhiJuqqnMgQjSiivBjV7DGCTf4XBBH/B7uvZsKxXShF0L8uDISWUvcExixRs6gB3oldSrjox6L8T94NOzqNCMEAwHQYDVR0OBBYEFEu9hyYRrRyJzwRYvnDSCIxrFiO3MA8GA1UdEwEB/wQFMAMBAf8wDgYDVR0PAQH/BAQDAgEGMAoGCCqGSM49BAMCA0gAMEUCIDHSb2mbNDAUNXvpPU0oWKeNye0fQ2l9D01AR2+sLZdhAiEAo3wz684IFMVsCCRmuJqxH6FQRESNqezuo1E+KkGxWuM="))
            };

            var trustPath = new X509Certificate2[2]
            {
                new X509Certificate2(Convert.FromBase64String("MIICQzCCAemgAwIBAgIQHfK1WlHcS2iFo9meaX/tFjAKBggqhkjOPQQDAjBJMQswCQYDVQQGEwJVUzEdMBsGA1UECgwURmVpdGlhbiBUZWNobm9sb2dpZXMxGzAZBgNVBAMMEkZlaXRpYW4gRklETyBDQSAwMzAgFw0xODEyMjUwMDAwMDBaGA8yMDMzMTIyNDIzNTk1OVowcDELMAkGA1UEBhMCVVMxHTAbBgNVBAoMFEZlaXRpYW4gVGVjaG5vbG9naWVzMSIwIAYDVQQLDBlBdXRoZW50aWNhdG9yIEF0dGVzdGF0aW9uMR4wHAYDVQQDDBVGVCBCaW9QYXNzIEZJRE8yIDA0NzAwWTATBgcqhkjOPQIBBggqhkjOPQMBBwNCAAS62hIbyenH9WPnzYHehaBR3C7qswomZkaPzGyUlFRiJIMo3uITeImFOFfNcDuOzoq1wcXXGTmEtEtxF2wo9noko4GJMIGGMB0GA1UdDgQWBBSBI1XoLDY1/HJaba+W32nxhxp3WjAfBgNVHSMEGDAWgBRBt/xNdcqO0p8s0xebzYNRinnYqTAMBgNVHRMBAf8EAjAAMBMGCysGAQQBguUcAgEBBAQDAgRwMCEGCysGAQQBguUcAQEEBBIEEBLe10VL7UfUq6rnE/UdY5MwCgYIKoZIzj0EAwIDSAAwRQIhAI6GSVi10r673uqtso+2oB6f5S5gE0ff44t3NcQ+TN9NAiAC/SCP+eKw1BnmcSgbxcQpYuWjBPMVDfqeg8pbmOdHKw==")),
                new X509Certificate2(Convert.FromBase64String("MIIB+TCCAaCgAwIBAgIQGBUrQbdDrm20FZnDsX2CCDAKBggqhkjOPQQDAjBLMQswCQYDVQQGEwJVUzEdMBsGA1UECgwURmVpdGlhbiBUZWNobm9sb2dpZXMxHTAbBgNVBAMMFEZlaXRpYW4gRklETyBSb290IENBMCAXDTE4MDUyMDAwMDAwMFoYDzIwMzgwNTE5MjM1OTU5WjBJMQswCQYDVQQGEwJVUzEdMBsGA1UECgwURmVpdGlhbiBUZWNobm9sb2dpZXMxGzAZBgNVBAMMEkZlaXRpYW4gRklETyBDQSAwMzBZMBMGByqGSM49AgEGCCqGSM49AwEHA0IABJts1KYQuj66rAszKKLfsOay91gO11vSvfcYd/dQfeTjpSNb55ffoLijQbRXspqE5Uj2NVylED61pjo2tpytOfijZjBkMB0GA1UdDgQWBBRBt/xNdcqO0p8s0xebzYNRinnYqTAfBgNVHSMEGDAWgBRLvYcmEa0cic8EWL5w0giMaxYjtzASBgNVHRMBAf8ECDAGAQH/AgEAMA4GA1UdDwEB/wQEAwIBBjAKBggqhkjOPQQDAgNHADBEAiAnSuhaqHgV3Sds/OrwiqLNUWMmU8Lji9Vy7s5hSEg22AIgE1lIdBjq0N+QcZq995uOE4XWxBIrVUio3RAwgDn8KgI="))
            };
            Assert.True(CryptoUtils.ValidateTrustChain(trustPath, attestationRootCertificates));
            Assert.False(CryptoUtils.ValidateTrustChain(trustPath, trustPath));
            Assert.False(CryptoUtils.ValidateTrustChain(attestationRootCertificates, attestationRootCertificates));
            Assert.False(CryptoUtils.ValidateTrustChain(attestationRootCertificates, trustPath));
        }
        //[Fact]
        public void TestValidateTrustChainSubAnchor()
        {
            byte[] attRootCertBytes = Convert.FromBase64String("MIIDCDCCAq+gAwIBAgIQQAFqUNTHZ8kBN8u/bCk+xDAKBggqhkjOPQQDAjBrMQswCQYDVQQGEwJVUzETMBEGA1UEChMKSElEIEdsb2JhbDEiMCAGA1UECxMZQXV0aGVudGljYXRvciBBdHRlc3RhdGlvbjEjMCEGA1UEAxMaRklETyBBdHRlc3RhdGlvbiBSb290IENBIDEwHhcNMTkwNDI0MTkzMTIzWhcNNDQwNDI3MTkzMTIzWjBmMQswCQYDVQQGEwJVUzETMBEGA1UEChMKSElEIEdsb2JhbDEiMCAGA1UECxMZQXV0aGVudGljYXRvciBBdHRlc3RhdGlvbjEeMBwGA1UEAxMVRklETyBBdHRlc3RhdGlvbiBDQSAyMFkwEwYHKoZIzj0CAQYIKoZIzj0DAQcDQgAE4nK9ctzk6GEGFNQBcrnBBmWU+dCnuHQAARrB2Eyc8MbsljkSFhZtfz/Rw6SuVIDk5VakDzrKBAOJ9v0Rvg/406OCATgwggE0MBIGA1UdEwEB/wQIMAYBAf8CAQAwDgYDVR0PAQH/BAQDAgGGMIGEBggrBgEFBQcBAQR4MHYwLgYIKwYBBQUHMAGGImh0dHA6Ly9oaWQuZmlkby5vY3NwLmlkZW50cnVzdC5jb20wRAYIKwYBBQUHMAKGOGh0dHA6Ly92YWxpZGF0aW9uLmlkZW50cnVzdC5jb20vcm9vdHMvSElERklET1Jvb3RjYTEucDdjMB8GA1UdIwQYMBaAFB2m3iwWSYHvWTHbJiHAyKDp+CSjMEcGA1UdHwRAMD4wPKA6oDiGNmh0dHA6Ly92YWxpZGF0aW9uLmlkZW50cnVzdC5jb20vY3JsL0hJREZJRE9Sb290Y2ExLmNybDAdBgNVHQ4EFgQUDLCbuLslcclrOZIz57Fu0imSMQ8wCgYIKoZIzj0EAwIDRwAwRAIgDCW5IrbjEI/y35lPjx9a+/sF4lPSoZdBHgFgTWC+8VICIEqs2SPzUHgHVh65Ajl1oIUmhh0C2lyR/Zdk7O3u1TIK");
            

            byte[] other = Convert.FromBase64String("MIICFzCCAbygAwIBAgIQQAFqUNQ4L2cr8xJ1985DBTAKBggqhkjOPQQDAjBrMQswCQYDVQQGEwJVUzETMBEGA1UEChMKSElEIEdsb2JhbDEiMCAGA1UECxMZQXV0aGVudGljYXRvciBBdHRlc3RhdGlvbjEjMCEGA1UEAxMaRklETyBBdHRlc3RhdGlvbiBSb290IENBIDEwHhcNMTkwNDI0MTkzMDQ2WhcNNDkwNDI0MTkzMDQ2WjBrMQswCQYDVQQGEwJVUzETMBEGA1UEChMKSElEIEdsb2JhbDEiMCAGA1UECxMZQXV0aGVudGljYXRvciBBdHRlc3RhdGlvbjEjMCEGA1UEAxMaRklETyBBdHRlc3RhdGlvbiBSb290IENBIDEwWTATBgcqhkjOPQIBBggqhkjOPQMBBwNCAAQBj82UGyUxgvIYnw3s6HVZDXRdKbrSnDbq4K1/dAO29Ntpt2UfD/TEwO6BV5cShQAnPUoTllHxefNAH0cm1eaRo0IwQDAPBgNVHRMBAf8EBTADAQH/MA4GA1UdDwEB/wQEAwIBhjAdBgNVHQ4EFgQUHabeLBZJge9ZMdsmIcDIoOn4JKMwCgYIKoZIzj0EAwIDSQAwRgIhAJqxO+TS3UMnmR4wtKCdkKiDqUHaPpXXraDgVS2VApUFAiEAsXKIsAeCl+4Pj2ivcl3ETiXVIuVTBODzU2aDzGEeRxw=");

            //var attestationRootCertificates = new X509Certificate2[2] { new X509Certificate2(attRootCertBytes), new X509Certificate2(other) };

            var attestationRootCertificates = new X509Certificate2[1] { new X509Certificate2(attRootCertBytes) };

            byte[] attCert = Convert.FromBase64String("MIIDLjCCAtSgAwIBAgIQQAFs2JXwQcL5Eh4rnp2ASjAKBggqhkjOPQQDAjBmMQswCQYDVQQGEwJVUzETMBEGA1UEChMKSElEIEdsb2JhbDEiMCAGA1UECxMZQXV0aGVudGljYXRvciBBdHRlc3RhdGlvbjEeMBwGA1UEAxMVRklETyBBdHRlc3RhdGlvbiBDQSAyMB4XDTE5MDgyODE0MTY0MFoXDTM5MDgyMzE0MTY0MFowaTELMAkGA1UEBhMCVVMxHzAdBgNVBAoTFkhJRCBHbG9iYWwgQ29ycG9yYXRpb24xIjAgBgNVBAsTGUF1dGhlbnRpY2F0b3IgQXR0ZXN0YXRpb24xFTATBgNVBAMTDENyZXNjZW5kb0tleTBZMBMGByqGSM49AgEGCCqGSM49AwEHA0IABAGouI654w6qbGonSTStO2cESYTo8Ezr8OJiPkMl02d6K6i44wXCKV2i+w+bpR6vgYQZ/cKQxMS4uGytqPRNPIejggFfMIIBWzAOBgNVHQ8BAf8EBAMCB4AwgYAGCCsGAQUFBwEBBHQwcjAuBggrBgEFBQcwAYYiaHR0cDovL2hpZC5maWRvLm9jc3AuaWRlbnRydXN0LmNvbTBABggrBgEFBQcwAoY0aHR0cDovL3ZhbGlkYXRpb24uaWRlbnRydXN0LmNvbS9jZXJ0cy9oaWRmaWRvY2EyLnA3YzAfBgNVHSMEGDAWgBQMsJu4uyVxyWs5kjPnsW7SKZIxDzAJBgNVHRMEAjAAMEMGA1UdHwQ8MDowOKA2oDSGMmh0dHA6Ly92YWxpZGF0aW9uLmlkZW50cnVzdC5jb20vY3JsL2hpZGZpZG9jYTIuY3JsMBMGCysGAQQBguUcAgEBBAQDAgQwMB0GA1UdDgQWBBR9h/lCWeTiMUhRS1tj31hBXaOurzAhBgsrBgEEAYLlHAEBBAQSBBBpLbVJeuVE1aHl3SCkk7cjMAoGCCqGSM49BAMCA0gAMEUCIQDpDa1ZbAfCTlBMiDUuB5XH8hnhZUF1JCuCmc+ShI4ZTwIga/ApAudL5R8HxOOHgk8AA/JpgCkMmYDQLVq0QF6oxrU=");
            var trustPath = new X509Certificate2[1] { new X509Certificate2(attCert) };

            Assert.False(0 == attestationRootCertificates[0].Issuer.CompareTo(attestationRootCertificates[0].Subject));
            Assert.True(CryptoUtils.ValidateTrustChain(trustPath, attestationRootCertificates));
            Assert.False(CryptoUtils.ValidateTrustChain(trustPath, trustPath));
            Assert.False(CryptoUtils.ValidateTrustChain(attestationRootCertificates, attestationRootCertificates));
            Assert.False(CryptoUtils.ValidateTrustChain(attestationRootCertificates, trustPath));
        }
        [Fact]
        public void TestValidateTrustChainSelf()
        {
            byte[] certBytes = Convert.FromBase64String("MIIBzTCCAXOgAwIBAgIJALS3SibGDXTPMAoGCCqGSM49BAMCMDsxIDAeBgNVBAMMF0dvVHJ1c3QgRklETzIgUm9vdCBDQSAxMRcwFQYDVQQKDA5Hb1RydXN0SUQgSW5jLjAeFw0xOTEyMDQwNjU5NDBaFw00OTExMjYwNjU5NDBaMDsxIDAeBgNVBAMMF0dvVHJ1c3QgRklETzIgUm9vdCBDQSAxMRcwFQYDVQQKDA5Hb1RydXN0SUQgSW5jLjBZMBMGByqGSM49AgEGCCqGSM49AwEHA0IABA5mjYsjowAI0jnpi//CJ3KnzhGbTUmstNWqN78ioG1CTK9gPgPl9UiFOJO/v+FfFK+Pxv10c604dvlIDAbKw+ijYDBeMAwGA1UdEwEB/wQCMAAwDgYDVR0PAQH/BAQDAgEGMB0GA1UdDgQWBBSgWtY0nEcmPmGDLuCwceKeJPScozAfBgNVHSMEGDAWgBSgWtY0nEcmPmGDLuCwceKeJPScozAKBggqhkjOPQQDAgNIADBFAiAxoVs6qj7DX2xixCjjcDUdxBTJmSTLb0f1rRGwrABzTQIhAPt0P32qzAeepF4//tgzxqNoKkWDcaPPSXrg+xzrlVHw");
            var certs = new X509Certificate2[1] { new X509Certificate2(certBytes) };
            
            byte[] otherCertBytes = Convert.FromBase64String("MIIDRjCCAu2gAwIBAgIUZPhSDtxI5lg2qgy+7IGDJhGqPOgwCgYIKoZIzj0EAwIwgYcxCzAJBgNVBAYTAlRXMQ8wDQYDVQQIDAZUYWlwZWkxEjAQBgNVBAcMCVNvbWV3aGVyZTEWMBQGA1UECgwNV2lTRUNVUkUgSW5jLjEgMB4GCSqGSIb3DQEJARYRYWRtaW5AZXhhbXBsZS5vcmcxGTAXBgNVBAMMEFdpU0VDVVJFIFJvb3QgQ0EwHhcNMjEwMTI4MDgyNzIwWhcNMzEwMTI2MDgyNzIwWjCBhzELMAkGA1UEBhMCVFcxDzANBgNVBAgMBlRhaXBlaTESMBAGA1UEBwwJU29tZXdoZXJlMRYwFAYDVQQKDA1XaVNFQ1VSRSBJbmMuMSAwHgYJKoZIhvcNAQkBFhFhZG1pbkBleGFtcGxlLm9yZzEZMBcGA1UEAwwQV2lTRUNVUkUgUm9vdCBDQTBZMBMGByqGSM49AgEGCCqGSM49AwEHA0IABBiWvFaf/IhFMOWNqlweqr4GfO0mu/1B18J03OG+pSltRix9GjRojBya4LARyXMP8nw2Xh9PvwOBm9QedMC66XGjggEzMIIBLzAdBgNVHQ4EFgQUd+Yvj6I3Y8cKH3QRNLlC8/Op97cwgccGA1UdIwSBvzCBvIAUd+Yvj6I3Y8cKH3QRNLlC8/Op97ehgY2kgYowgYcxCzAJBgNVBAYTAlRXMQ8wDQYDVQQIDAZUYWlwZWkxEjAQBgNVBAcMCVNvbWV3aGVyZTEWMBQGA1UECgwNV2lTRUNVUkUgSW5jLjEgMB4GCSqGSIb3DQEJARYRYWRtaW5AZXhhbXBsZS5vcmcxGTAXBgNVBAMMEFdpU0VDVVJFIFJvb3QgQ0GCFGT4Ug7cSOZYNqoMvuyBgyYRqjzoMAwGA1UdEwEB/wQCMAAwNgYDVR0fBC8wLTAroCmgJ4YlaHR0cDovL3d3dy5leGFtcGxlLm9yZy9leGFtcGxlX2NhLmNybDAKBggqhkjOPQQDAgNHADBEAiBf3p8LJ3PlfMsxTzWgjHaal6uzIo5tx3o+EUybdDY4ogIgV6nR1MUE1wKz1uC7/kENg/FpJOetFaJePcgoneEwsKA=");
            var otherCerts = new X509Certificate2[1] { new X509Certificate2(otherCertBytes) };

            Assert.True(CryptoUtils.ValidateTrustChain(certs, certs));
            Assert.False(CryptoUtils.ValidateTrustChain(certs, otherCerts));
        }
    }
}
