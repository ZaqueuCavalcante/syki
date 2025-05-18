namespace Syki.Back.Settings;

public class SentrySettings
{
    public string Dsn { get; set; }
    public double TracesSampleRate { get; set; }

    public SentrySettings(IConfiguration configuration)
    {
        configuration.GetSection("Sentry").Bind(this);
    }
}
