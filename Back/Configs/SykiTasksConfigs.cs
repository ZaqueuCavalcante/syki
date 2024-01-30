using Syki.Back.Tasks;

namespace Syki.Back.Configs;

public static class SykiTasksConfigs
{
    public static void AddSykiTasksConfigs(this IServiceCollection services)
    {
        services.AddTransient<SendResetPasswordEmailHandler>();

        services.AddHostedService<SykiTasksProcessor>();
    }
}
