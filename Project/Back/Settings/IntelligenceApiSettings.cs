namespace Exato.Back.Settings;

public class IntelligenceApiSettings
{
    public string Url { get; set; }
    public string Token { get; set; }

    public IntelligenceApiSettings() { }

    public IntelligenceApiSettings(IConfiguration configuration)
    {
        configuration.GetSection("IntelligenceApi").Bind(this);
    }
}

public static class IntelligenceApiSettingsExtensions
{
    extension(IConfiguration configuration)
    {
        public IntelligenceApiSettings IntelligenceApi => new(configuration);
    }
}
