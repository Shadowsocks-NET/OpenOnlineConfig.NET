using OpenOnlineConfig.Utils;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;

namespace OpenOnlineConfig.v1
{
    /// <summary>
    /// Generic OOCv1 API client.
    /// </summary>
    /// <typeparam name="T">OOCv1 config type for specific protocols.</typeparam>
    public class OOCv1ApiClient<T> : OOCv1ApiClient where T : OOCv1ConfigBase
    {
        public OOCv1ApiClient(OOCv1ApiToken apiToken, HttpClient? httpClient = null) : base(apiToken, httpClient) { }

        /// <summary>
        /// Retrieves the online config and deserializes the response JSON as <see cref="T"/> in an asynchronous operation.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token to cancel the operation.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public new Task<T?> GetAsync(CancellationToken cancellationToken = default)
            => _httpClient.GetFromJsonAsync<T>($"{_apiToken.BaseUrl}/{_apiToken.Secret}/ooc/v1/{_apiToken.UserId}", JsonHelper.camelCaseJsonDeserializerOptions, cancellationToken);
    }
}
