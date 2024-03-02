using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

namespace Syki.Front.Configs;

public static class ServicesConfigs
{
    public static void AddServicesConfigs(this WebAssemblyHostBuilder builder)
    {
        builder.Services.AddScoped<CreatePendingUserRegisterClient>();
        builder.Services.AddScoped<FinishUserRegisterClient>();
        builder.Services.AddScoped<GetMfaKeyClient>();
        builder.Services.AddScoped<SetupMfaClient>();

        builder.Services.AddScoped<CreateAcademicPeriodClient>();
        builder.Services.AddScoped<GetAcademicPeriodsClient>();
        builder.Services.AddScoped<CreateEnrollmentPeriodClient>();
        builder.Services.AddScoped<GetEnrollmentPeriodsClient>();
        builder.Services.AddScoped<GetCurrentEnrollmentPeriodClient>();
    }
}
