using System.Net.Http.Json;
using System.Security.Cryptography;
using System.Text.Json.Serialization.Metadata;

namespace OpenOnlineConfig.v1
{
    /// <summary>
    /// General-purpose OOCv1 API client.
    /// </summary>
    public class OOCv1ApiClient : IDisposable
    {
        protected readonly HttpClient _httpClient;
        private readonly Uri _uri;
        private readonly bool _disposeHttpClient;
        private bool disposedValue;

        /// <summary>
        /// Creates a general-purpose OOCv1 API client for the API endpoint.
        /// The supplied HttpClient is used only when the API doesn't use a self-signed certificate.
        /// </summary>
        /// <param name="apiToken">API token of the designated API endpoint.</param>
        /// <param name="httpClient">Supply an existing HttpClient instance.</param>
        public OOCv1ApiClient(OOCv1ApiToken apiToken, HttpClient? httpClient = null)
        {
            _uri = apiToken.UserUrl;

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
                        RemoteCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) =>
                        {
                            string? sha256Fingerprint = certificate?.GetCertHashString(HashAlgorithmName.SHA256);
                            return string.Equals(apiToken.CertSha256, sha256Fingerprint, StringComparison.OrdinalIgnoreCase);
                        },
                    },
                };

                _httpClient = new(socketsHttpHandler);
                _disposeHttpClient = true;
            }
        }

        /// <inheritdoc cref="HttpClient.Timeout"/>
        public TimeSpan Timeout
        {
            get => _httpClient.Timeout;
            set => _httpClient.Timeout = value;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposeHttpClient && !disposedValue)
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
        /// Retrieves the online config and deserializes the response JSON as <typeparamref name="TValue"/> in an asynchronous operation.
        /// </summary>
        /// <param name="jsonTypeInfo">Metadata about the type to convert.</param>
        /// <param name="cancellationToken">The cancellation token to cancel the operation.</param>
        /// <returns>The <see cref="Task"/> object representing the asynchronous operation.</returns>
        public Task<TValue?> GetAsync<TValue>(JsonTypeInfo<TValue> jsonTypeInfo, CancellationToken cancellationToken = default) where TValue : OOCv1ConfigBase =>
            _httpClient.GetFromJsonAsync(_uri, jsonTypeInfo, cancellationToken);
    }
}
