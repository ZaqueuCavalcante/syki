using Syki.Daemon.Settings;

namespace Syki.Daemon;

public static class Extensions
{
    public static DatabaseSettings Database(this IConfiguration configuration) => new(configuration);
    public static TasksSettings Tasks(this IConfiguration configuration) => new(configuration);
    public static HangfireSettings Hangfire(this IConfiguration configuration) => new(configuration);
}
