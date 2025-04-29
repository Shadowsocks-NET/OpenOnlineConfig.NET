using System;

namespace OpenOnlineConfig.v1
{
    /// <summary>
    /// OOCv1 API access information.
    /// Serialize and deserialize in camelCase.
    /// </summary>
    public record OOCv1ApiToken(int Version, string BaseUrl, string Secret, string UserId, string? CertSha256)
    {
        /// <summary>
        /// Gets the API URL for the user.
        /// </summary>
        public Uri UserUrl => new($"{BaseUrl.TrimEnd('/')}/{Secret}/ooc/v1/{UserId}");
    }
}
