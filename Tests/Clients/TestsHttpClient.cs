using System.Text.Json;
using System.Text.Json.Serialization;
using Syki.Back.Features.Users.RegisterUser;

namespace Syki.Tests.Integration.Clients;

public partial class TestsHttpClient(HttpClient http)
{
    public RegisterUserOut User { get; set; }

    public static JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        Converters = { new JsonStringEnumConverter() }
    };
}
