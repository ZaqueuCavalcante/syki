using Syki.Daemon.Settings;

namespace Syki.Daemon.Extensions;

public static class Extensions
{
    public static AuditSettings Audit(this IConfiguration configuration) => new(configuration);
}
