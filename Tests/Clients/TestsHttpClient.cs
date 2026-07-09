using System.Text.Json;
using System.Text.Json.Serialization;

namespace Estud.Tests.Integration.Clients;

public partial class TestsHttpClient(HttpClient http)
{
    public TestsUserDto User { get; set; }

    public static JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        Converters = { new JsonStringEnumConverter() }
    };
}
