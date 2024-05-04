using Syki.Front.Auth;
using Microsoft.AspNetCore.Components.Authorization;

namespace Syki.Front.Configs;

public static class AuthConfigs
{
    public static void AddAuthConfigs(this WebAssemblyHostBuilder builder)
    {
        builder.Services.AddAuthorizationCore();
        builder.Services.AddScoped<SykiAuthStateProvider>();
        builder.Services.AddScoped<AuthenticationStateProvider>(sp => sp.GetRequiredService<SykiAuthStateProvider>());
    }
}
