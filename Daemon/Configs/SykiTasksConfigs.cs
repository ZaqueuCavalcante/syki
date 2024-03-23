using Syki.Back.Settings;
using Microsoft.Extensions.DependencyInjection;
using Syki.Daemon.Settings;
using Syki.Daemon.Tasks;

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
