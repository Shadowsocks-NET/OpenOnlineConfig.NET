using System;
using System.Net.Http;
using System.Net.Security;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;

namespace OpenOnlineConfig.v1
{
    /// <summary>
    /// General-purpose OOCv1 API client.
    /// </summary>
    public class OOCv1ApiClient : IDisposable
    {
        protected readonly OOCv1ApiToken _apiToken;
        protected readonly HttpClient _httpClient;
        private bool disposedValue;

        /// <summary>
        /// Creates a general-purpose OOCv1 API client for the API endpoint.
        /// The supplied HttpClient is used only when the API doesn't use a self-signed certificate.
        /// </summary>
        /// <param name="apiToken">API token of the designated API endpoint.</param>
        /// <param name="httpClient">Supply an existing HttpClient instance.</param>
        public OOCv1ApiClient(OOCv1ApiToken apiToken, HttpClient? httpClient = null)
        {
            _apiToken = apiToken;

            if (apiToken.CertSha256 is null)
            {
                _httpClient = httpClient ?? new();
            }
            else
            {
                SocketsHttpHandler socketsHttpHandler = new()
                {
                    SslOptions = new()
                    {
                        RemoteCertificateValidationCallback = ValidateServerCertificate,
                    },
                };

                _httpClient = new(socketsHttpHandler);
            }

            _httpClient.Timeout = TimeSpan.FromSeconds(30.0);
        }

        /// <inheritdoc cref="HttpClient.Timeout"/>
        public TimeSpan Timeout
        {
            get => _httpClient.Timeout;
            set => _httpClient.Timeout = value;
        }

        private bool ValidateServerCertificate(object sender, X509Certificate? certificate, X509Chain? chain, SslPolicyErrors sslPolicyErrors)
        {
            var sha256Fingerprint = certificate?.GetCertHashString(HashAlgorithmName.SHA256);
            return string.Equals(_apiToken.CertSha256, sha256Fingerprint, StringComparison.OrdinalIgnoreCase);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _httpClient.Dispose();
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Retrieves the online config and returns the response JSON as a string in an asynchronous operation.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token to cancel the operation.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public Task<string> GetAsync(CancellationToken cancellationToken = default)
            => _httpClient.GetStringAsync($"{_apiToken.BaseUrl}/{_apiToken.Secret}/ooc/v1/{_apiToken.UserId}", cancellationToken);
    }
}
