namespace Syki.Front.Configs;

public static class CrossServicesConfigs
{
    public static void AddCrossServicesConfigs(this WebAssemblyHostBuilder builder)
    {
        builder.Services.AddScoped<CreatePendingUserRegisterClient>();
        builder.Services.AddScoped<FinishUserRegisterClient>();
        builder.Services.AddScoped<GetMfaKeyClient>();
        builder.Services.AddScoped<GetUserNotificationsClient>();
        builder.Services.AddScoped<LoginClient>();
        builder.Services.AddScoped<LoginMfaClient>();
        builder.Services.AddScoped<ResetPasswordClient>();
        builder.Services.AddScoped<SendResetPasswordTokenClient>();
        builder.Services.AddScoped<SetupMfaClient>();
        builder.Services.AddScoped<ViewNotificationsClient>();
    }
}
