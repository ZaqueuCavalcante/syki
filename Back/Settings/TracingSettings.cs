namespace Syki.Back.Settings;

public class TracingSettings
{
    public double  SamplingRatio { get; set; }

    public TracingSettings() { }

    public TracingSettings(IConfiguration configuration)
    {
        configuration.GetSection("Tracing").Bind(this);
    }
}

public static class TracingSettingsExtensions
{
    public static TracingSettings Tracing(this IConfiguration configuration) => new(configuration);
}
