using Syki.Daemon.Tasks;
using Syki.Daemon.Settings;

namespace Syki.Daemon.Configs;

public static class SykiTasksConfigs
{
    public static void AddSykiTasksConfigs(this IServiceCollection services)
    {
        services.AddSingleton<EmailSettings>();
        services.AddSingleton<DatabaseSettings>();
        services.AddSingleton<HangfireSettings>();

        services.AddTransient<SykiTasksProcessor>();

        services.AddTransient<SeedInstitutionDataHandler>();
        services.AddTransient<LinkOldNotificationsHandler>();
        services.AddTransient<CreateNewExamGradeNoteNotificationHandler>();
    }
}
