namespace Exato.Back.Settings;

public class EmailSettings
{
    public string ApiKey { get; set; }
    public string FrontUrl { get; set; }

    public EmailSettings(IConfiguration configuration)
    {
        configuration.GetSection("Email").Bind(this);
    }
}

public static class EmailSettingsExtensions
{
    extension(IConfiguration configuration)
    {
        public EmailSettings Email => new(configuration);
    }
}
