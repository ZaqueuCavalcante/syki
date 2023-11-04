using Syki.Front.Auth;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

namespace Syki.Front.Configs;

public static class HttpConfigs
{
    public static void AddHttpConfigs(this WebAssemblyHostBuilder builder)
    {
        builder.Services.AddScoped<SykiDelegatingHandler>();

        builder.Services
            .AddHttpClient("HttpClient", x => x.BaseAddress = new Uri(builder.GetUrl()))
            .AddHttpMessageHandler<SykiDelegatingHandler>();

        builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>()
            .CreateClient("HttpClient"));
    }
}
