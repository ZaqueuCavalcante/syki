namespace Exato.Back.Settings;

public class OpenTelemetrySettings
{
    public bool Enabled { get; set; }
    public double TracingSamplingRatio { get; set; }

    public OpenTelemetrySettings() { }

    public OpenTelemetrySettings(IConfiguration configuration)
    {
        configuration.GetSection("OpenTelemetry").Bind(this);
    }
}

public static class OpenTelemetrySettingsExtensions
{
    extension(IConfiguration configuration)
    {
        public OpenTelemetrySettings OpenTelemetry => new(configuration);
    }
}
