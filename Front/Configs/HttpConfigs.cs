namespace Syki.Front.Configs;

public static class HttpConfigs
{
    public static void AddHttpConfigs(this WebAssemblyHostBuilder builder)
    {
        builder.Services.AddScoped<SykiDelegatingHandler>();

        var apiUrl = builder.Configuration.GetSection("ApiUrl").Value!;

        builder.Services
            .AddHttpClient("HttpClient", x => x.BaseAddress = new Uri(apiUrl))
            .AddHttpMessageHandler<SykiDelegatingHandler>();

        builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>()
            .CreateClient("HttpClient"));
    }
}
