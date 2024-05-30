namespace Syki.Daemon.Settings;

public class HangfireSettings
{
    public string User { get; set; }
    public string Password { get; set; }

    public HangfireSettings(IConfiguration configuration)
    {
        configuration.GetSection("Hangfire").Bind(this);
    }
}
