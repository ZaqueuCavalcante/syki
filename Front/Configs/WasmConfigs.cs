using Front;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

namespace Syki.Front.Configs;

public static class WasmConfigs
{
    public static WebAssemblyHostBuilder CreateHostBuilder(string[]? args = null)
    {
        var builder = WebAssemblyHostBuilder.CreateDefault(args);
        builder.RootComponents.Add<App>("#app");
        builder.RootComponents.Add<HeadOutlet>("head::after");
        return builder;
    }

    public static string GetUrl(this WebAssemblyHostBuilder builder)
    {
        return builder.HostEnvironment.IsDevelopment() ?
            "http://localhost:5160" : "https://syki-dev-api.azurewebsites.net";
    }
}
