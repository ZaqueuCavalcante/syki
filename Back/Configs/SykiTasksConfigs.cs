using Syki.Back.CreatePendingUserRegister;

namespace Syki.Back.Configs;

public static class SykiTasksConfigs
{
    public static void AddSykiTasksConfigs(this IServiceCollection services)
    {
        services.AddTransient<SeedInstitutionDemoDataHandler>();
        services.AddTransient<SendResetPasswordEmailHandler>();
        services.AddTransient<SendUserRegisterEmailConfirmationHandler>();

        services.AddHostedService<SykiTasksProcessor>();
    }
}
