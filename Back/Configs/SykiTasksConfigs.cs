using Syki.Back.Tasks;

namespace Syki.Back.Configs;

public static class SykiTasksConfigs
{
    public static void AddSykiTasksConfigs(this IServiceCollection services)
    {
        services.AddTransient<SeedFaculdadeTestDataHandler>();
        services.AddTransient<SendResetPasswordEmailHandler>();
        services.AddTransient<SendDemoEmailConfirmationHandler>();

        services.AddHostedService<SykiTasksProcessor>();
    }
}
