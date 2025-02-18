using System.Net.Security;

namespace TaskManagementApp.Frontends.Web.Configurations
{
    public class CertificatesValidator
    {
        private readonly List<X509Certificate2> _trustedCertificates;
        private readonly Logger _logger;

        public CertificatesValidator(List<X509Certificate2> trustedCertificates, Logger logger)
        {
            _trustedCertificates = trustedCertificates;
            _logger = logger;
        }

        public bool Validate(HttpRequestMessage message, X509Certificate2 cert, X509Chain chain, SslPolicyErrors errors)
        {
            if (errors == SslPolicyErrors.None)
                return true;

            if (errors != SslPolicyErrors.None)            
                _logger.Debug($"\n--------\nSSL errors: {errors}\n--------\n");

            bool isChainValid = chain?.Build(cert) == true;

            bool isCertsValid = _trustedCertificates.Any(trustedCert => cert.Thumbprint == trustedCert.Thumbprint);

            return isChainValid && isCertsValid;
        }
    }
}
