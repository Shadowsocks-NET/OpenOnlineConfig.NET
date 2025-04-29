# `OpenOnlineConfig.NET`

[![Nuget](https://img.shields.io/nuget/v/OpenOnlineConfig)](https://www.nuget.org/packages/OpenOnlineConfig/)
[![Build & Test](https://github.com/Shadowsocks-NET/OpenOnlineConfig.NET/actions/workflows/build.yml/badge.svg)](https://github.com/Shadowsocks-NET/OpenOnlineConfig.NET/actions/workflows/build.yml)
[![Publish NuGet Package](https://github.com/Shadowsocks-NET/OpenOnlineConfig.NET/actions/workflows/publish.yml/badge.svg)](https://github.com/Shadowsocks-NET/OpenOnlineConfig.NET/actions/workflows/publish.yml)

`OpenOnlineConfig.NET` is a .NET class library for Open Online Config (OOC) support.

## Supported Open Online Config Versions

- [OOCv1](https://github.com/Shadowsocks-NET/OpenOnlineConfig/blob/master/docs/0001-open-online-config-v1.md)

## Usage

### 1. API Token

`OOCv1ApiToken` is an immutable record type. It can be used to serialize and deserialize API tokens.

``` cs
// Serialize an API token.
OOCv1ApiToken token = new(1, "https://example.com", "8c1da4d8-8684-4a2c-9abb-57b9d5fa7e52", "a117460e-41df-4dbd-b2df-4bd0c16efd2f", null);
string json = JsonSerializer.Serialize(token, OOCv1JsonSerializerContext.Default.OOCv1ApiToken);
```

``` cs
// Deserialize an API token.
string json = "{\"version\":1,\"baseUrl\":\"https://example.com\",\"secret\":\"8c1da4d8-8684-4a2c-9abb-57b9d5fa7e52\",\"userId\":\"a117460e-41df-4dbd-b2df-4bd0c16efd2f\"}";
OOCv1ApiToken? token = JsonSerializer.Deserialize(json, OOCv1JsonSerializerContext.Default.OOCv1ApiToken);
```

### 2. Config Base

`OOCv1ConfigBase` is the minimal OOCv1 config type. To use this type, inherit from it and add protocol-specific properties.

### 3. API Client

`OOCv1ApiClient` is the general-purpose OOCv1 API client. Call `GetAsync<TValue>` to retrieve the online config. `TValue` must be a subclass of `OOCv1ConfigBase`.

## License

The project is licensed under the [MIT license](LICENSE).
