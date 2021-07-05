# `OpenOnlineConfig.NET`

![Nuget](https://img.shields.io/nuget/v/OpenOnlineConfig)

`OpenOnlineConfig.NET` is a .NET class library for Open Online Config (OOC) support.

## Supported Open Online Config Versions

- [OOCv1](https://github.com/Shadowsocks-NET/OpenOnlineConfig/blob/master/docs/0001-open-online-config-v1.md)

## Usage

### 1. API Token

`OOCv1ApiToken` is an immutable record type. It can be used to serialize and deserialize API tokens.

``` cs
// Serialize an API token.
var token = new OOCv1ApiToken(1, "https://example.com", "8c1da4d8-8684-4a2c-9abb-57b9d5fa7e52", "a117460e-41df-4dbd-b2df-4bd0c16efd2f", null);
var json = JsonSerializer.Serialize(token, JsonHelper.camelCaseMinifiedJsonSerializerOptions);
```

``` cs
// Deserialize an API token.
var json = "{\"version\":1,\"baseUrl\":\"https://example.com\",\"secret\":\"8c1da4d8-8684-4a2c-9abb-57b9d5fa7e52\",\"userId\":\"a117460e-41df-4dbd-b2df-4bd0c16efd2f\"}";
var token = JsonSerializer.Deserialize<OOCv1ApiToken>(json, JsonHelper.camelCaseJsonDeserializerOptions);
```

### 2. Config Base

`OOCv1ConfigBase` is the minimal OOCv1 config type. To use this type, inherit from it and add protocol-specific properties.

### 3. API Client

`OOCv1ApiClient` is the general-purpose OOCv1 API client. Use this if you want to handle the response JSON yourself.

`OOCv1ApiClient<T>` is the generic OOCv1 API client. Use this if you have specific protocols in mind. `T` must be a subclass of `OOCv1ConfigBase`.

## License

The project is licensed under the [MIT license](LICENSE).
