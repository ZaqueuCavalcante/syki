namespace Syki.Daemon.Configs;

public static class AuditConfigs
{
    public static void AddDaemonAuditConfigs(this WebApplicationBuilder builder)
    {
        Audit.Core.Configuration.AuditDisabled = builder.Configuration.Audit().Disabled;
    }
}
