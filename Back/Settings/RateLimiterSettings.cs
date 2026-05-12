namespace Syki.Back.Settings;

public class RateLimiterSettings : SettingsBase
{
    public int SuperVerySmall { get; set; }
    public int VerySmall { get; set; }
    public int Small { get; set; }
    public int Medium { get; set; }

    public RateLimiterSettings(IConfiguration configuration)
    {
        configuration.GetSection("RateLimiter").Bind(this);

        RequirePositive(SuperVerySmall);
        RequirePositive(VerySmall);
        RequirePositive(Small);
        RequirePositive(Medium);
    }
}
