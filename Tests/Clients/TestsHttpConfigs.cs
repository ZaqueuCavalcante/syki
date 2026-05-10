using System.Text.Json;
using System.Text.Json.Serialization;

namespace Syki.Tests.Clients;

public static class HttpConfigs
{
    public static JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        Converters = { new JsonStringEnumConverter() }
    };
}
