namespace Estud.Back.Settings;

public class RateLimitingSettings : SettingsBase
{
    public int QueueLimit { get; set; } = 0;
    public int GlobalPermitLimit { get; set; } = 100;
    public int GlobalWindowInSeconds { get; set; } = 60;

    public int SensitivePermitLimit { get; set; } = 10;
    public int SensitiveWindowInSeconds { get; set; } = 60;

    public RateLimitingSettings(IConfiguration configuration)
    {
        configuration.GetSection("RateLimiting").Bind(this);
    }
}

public static class RateLimitingSettingsExtensions
{
    extension(IConfiguration configuration)
    {
        public RateLimitingSettings RateLimiting => new(configuration);
    }
}
