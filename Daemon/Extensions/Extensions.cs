using Syki.Daemon.Settings;

namespace Syki.Daemon.Extensions;

public static class Extensions
{
    public static DatabaseSettings Database(this IConfiguration configuration) => new(configuration);
    public static HangfireSettings Hangfire(this IConfiguration configuration) => new(configuration);
    public static AuditSettings Audit(this IConfiguration configuration) => new(configuration);
}
