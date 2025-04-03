namespace Syki.Daemon.Settings;

public class AuditSettings
{
    public bool Disabled { get; set; }

    public AuditSettings(IConfiguration configuration)
    {
        configuration.GetSection("Audit").Bind(this);
    }
}
