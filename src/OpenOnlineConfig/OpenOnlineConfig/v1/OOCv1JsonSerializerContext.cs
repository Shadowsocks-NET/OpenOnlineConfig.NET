using System.Text.Json.Serialization;

namespace OpenOnlineConfig.v1;

[JsonSerializable(typeof(OOCv1ApiToken))]
[JsonSerializable(typeof(OOCv1ConfigBase))]
[JsonSourceGenerationOptions(
    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault,
    IgnoreReadOnlyProperties = true,
    PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase)]
public partial class OOCv1JsonSerializerContext : JsonSerializerContext
{
}
