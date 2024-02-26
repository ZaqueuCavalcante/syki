using Syki.Back.FinishUserRegister;
using Syki.Back.CreatePendingUserRegister;

namespace Syki.Back.Configs;

public static class SykiTasksConfigs
{
    public static void AddSykiTasksConfigs(this IServiceCollection services)
    {
        services.AddTransient<SeedInstitutionDataHandler>();
        services.AddTransient<SendResetPasswordEmailHandler>();
        services.AddTransient<SendUserRegisterEmailConfirmationHandler>();

        services.AddHostedService<SykiTasksProcessor>();
    }
}
