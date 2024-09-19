using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;

namespace Syki.Front.Configs;

public static class AuthConfigs
{
    public static void AddAuthConfigs(this WebAssemblyHostBuilder builder)
    {
        builder.Services.AddScoped<IAuthorizationHandler, SkipUserRegisterAuthReqHandler>();

        builder.Services.AddAuthorizationCore(options =>
        {
            options.AddPolicy(FrontPolicy.SkipUserRegister, p => p.Requirements.Add(new SkipUserRegisterAuthReq()));
        });

        builder.Services.AddScoped<SykiAuthStateProvider>();
        builder.Services.AddScoped<AuthenticationStateProvider>(sp => sp.GetRequiredService<SykiAuthStateProvider>());
    }
}
