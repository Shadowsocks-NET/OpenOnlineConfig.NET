using OpenOnlineConfig.v1;
using System.Text.Json;
using Xunit;

namespace OpenOnlineConfig.Tests
{
    public class DeserializationTests
    {
        [Theory]
        [InlineData("{\"version\":1,\"baseUrl\":\"https://example.com\",\"secret\":\"8c1da4d8-8684-4a2c-9abb-57b9d5fa7e52\",\"userId\":\"a117460e-41df-4dbd-b2df-4bd0c16efd2f\"}",
            1, "https://example.com", "8c1da4d8-8684-4a2c-9abb-57b9d5fa7e52", "a117460e-41df-4dbd-b2df-4bd0c16efd2f", null)]
        [InlineData("{\"version\":1,\"baseUrl\":\"https://github.com\",\"secret\":\"8b7ef7fd-c2ac-47d9-8517-2b20c4fee5a7\",\"userId\":\"68720c88-e5e5-4b84-9db7-98fb4be109eb\",\"certSha256\":\"0ae384bfd4dde9d13e50c5857c05a442c93f8e01445ee4b34540d22bd1e37f1b\"}",
            1, "https://github.com", "8b7ef7fd-c2ac-47d9-8517-2b20c4fee5a7", "68720c88-e5e5-4b84-9db7-98fb4be109eb", "0ae384bfd4dde9d13e50c5857c05a442c93f8e01445ee4b34540d22bd1e37f1b")]
        public void ApiToken_Deserialization(string json, int expectedVersion, string expectedBaseUrl, string expectedSecret, string expectedUserId, string? expectedCertSha256)
        {
            OOCv1ApiToken? token = JsonSerializer.Deserialize(json, OOCv1JsonSerializerContext.Default.OOCv1ApiToken);

            Assert.NotNull(token);
            Assert.Equal(expectedVersion, token.Version);
            Assert.Equal(expectedBaseUrl, token.BaseUrl);
            Assert.Equal(expectedSecret, token.Secret);
            Assert.Equal(expectedUserId, token.UserId);
            Assert.Equal(expectedCertSha256, token.CertSha256);
        }

        [Theory]
        [InlineData("{\"username\":\"somebody\",\"protocols\":[\"shadowsocks\"]}",
            "somebody", null, null, null, new string[] { "shadowsocks", })]
        [InlineData("{\"username\":\"nobody\",\"bytesUsed\":274877906944,\"bytesRemaining\":824633720832,\"expiryDate\":1609459200,\"protocols\":[\"shadowsocks\",\"vmess\",\"trojan-go\"]}",
            "nobody", 274877906944UL, 824633720832UL, 1609459200L, new string[] { "shadowsocks", "vmess", "trojan-go", })]
        public void ConfigBase_Deserialization(string json, string? expectedUsername, ulong? expectedBytesUsed, ulong? expectedBytesRemaining, long? expectedExpiryDate, string[] expectedProtocols)
        {
            OOCv1ConfigBase? config = JsonSerializer.Deserialize(json, OOCv1JsonSerializerContext.Default.OOCv1ConfigBase);

            Assert.NotNull(config);
            Assert.Equal(expectedUsername, config.Username);
            Assert.Equal(expectedBytesUsed, config.BytesUsed);
            Assert.Equal(expectedBytesRemaining, config.BytesRemaining);
            Assert.Equal(expectedExpiryDate, config.ExpiryDate?.ToUnixTimeSeconds());
            Assert.Equal(expectedProtocols, config.Protocols);
        }
    }
}
