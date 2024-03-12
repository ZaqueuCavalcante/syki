namespace Syki.Front.Configs;

public static class ServicesConfigs
{
    public static void AddServicesConfigs(this WebAssemblyHostBuilder builder)
    {
        builder.Services.AddScoped<CreatePendingUserRegisterClient>();
        builder.Services.AddScoped<FinishUserRegisterClient>();
        builder.Services.AddScoped<LoginClient>();
        builder.Services.AddScoped<LoginMfaClient>();
        builder.Services.AddScoped<GetMfaKeyClient>();
        builder.Services.AddScoped<SetupMfaClient>();
        builder.Services.AddScoped<SendResetPasswordTokenClient>();
        builder.Services.AddScoped<ResetPasswordClient>();
        builder.Services.AddScoped<GetAcademicInsightsClient>();

        builder.Services.AddScoped<CreateCampusClient>();

        builder.Services.AddScoped<CreateAcademicPeriodClient>();
        builder.Services.AddScoped<GetAcademicPeriodsClient>();
        builder.Services.AddScoped<CreateEnrollmentPeriodClient>();
        builder.Services.AddScoped<GetEnrollmentPeriodsClient>();
        builder.Services.AddScoped<GetCurrentEnrollmentPeriodClient>();
    }
}
