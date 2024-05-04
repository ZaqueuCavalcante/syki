using Syki.Daemon.Tasks;
using Syki.Back.Settings;
using Syki.Daemon.Settings;
using Microsoft.Extensions.DependencyInjection;

namespace Syki.Daemon.Configs;

public static class SykiTasksConfigs
{
    public static void AddSykiTasksConfigs(this IServiceCollection services)
    {
        services.AddSingleton<TasksSettings>();
        services.AddSingleton<EmailSettings>();
        services.AddSingleton<DatabaseSettings>();

        services.AddHostedService<SykiTasksProcessor>();

        services.AddTransient<SeedInstitutionDataHandler>();
        services.AddTransient<SendResetPasswordEmailHandler>();
        services.AddTransient<SendUserRegisterEmailConfirmationHandler>();
    }
}
