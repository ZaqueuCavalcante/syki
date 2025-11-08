using System.Text.Json;
using Exato.Front.Auth;
using System.Text.Json.Serialization;

namespace Exato.Front.Configs;

public static class HttpConfigs
{
    public static void AddHttpConfigs(this WebAssemblyHostBuilder builder)
    {
        builder.Services.AddScoped<ExatoDelegatingHandler>();

        var apiUrl = builder.Configuration.GetSection("ApiUrl").Value!;

        builder.Services
            .AddHttpClient("HttpClient", x =>
            {
                x.BaseAddress = new Uri(apiUrl);
                x.Timeout = TimeSpan.FromSeconds(120);
            })
            .AddHttpMessageHandler<ExatoDelegatingHandler>();

        builder.Services.AddTransient(sp => sp.GetRequiredService<IHttpClientFactory>()
            .CreateClient("HttpClient"));
    }

    public static JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        Converters = { new JsonStringEnumConverter() }
    };
}
