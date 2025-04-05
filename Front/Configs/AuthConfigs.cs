using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;

namespace Syki.Front.Configs;

public static class AuthConfigs
{
    public static void AddAuthConfigs(this WebAssemblyHostBuilder builder)
    {
        builder.Services.AddScoped<IAuthorizationHandler, CrossLoginAuthReqHandler>();

        builder.Services.AddAuthorizationCore(options =>
        {
            options.AddPolicy(FrontPolicy.CrossLogin, p => p.Requirements.Add(new CrossLoginAuthReq()));
        });

        builder.Services.AddScoped<AuthManager>();
        builder.Services.AddScoped<SykiAuthStateProvider>();
        builder.Services.AddScoped<AuthenticationStateProvider>(sp => sp.GetRequiredService<SykiAuthStateProvider>());
    }
}
