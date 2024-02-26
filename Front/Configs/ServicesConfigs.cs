using Syki.Front.Services;
using Syki.Front.CreatePendingUserRegister;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

namespace Syki.Front.Configs;

public static class ServicesConfigs
{
    public static void AddServicesConfigs(this WebAssemblyHostBuilder builder)
    {
        builder.Services.AddScoped<CreatePendingUserRegisterClient>();

        builder.Services.AddScoped<IMfaService, MfaService>();
    }
}
