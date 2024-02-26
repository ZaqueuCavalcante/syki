using Syki.Front.Services;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

namespace Syki.Front.Configs;

public static class ServicesConfigs
{
    public static void AddServicesConfigs(this WebAssemblyHostBuilder builder)
    {
        builder.Services.AddScoped<CreatePendingUserRegisterClient>();
        builder.Services.AddScoped<FinishUserRegisterClient>();
        builder.Services.AddScoped<GetMfaKeyClient>();

        builder.Services.AddScoped<IMfaService, MfaService>();
    }
}
