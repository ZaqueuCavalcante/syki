namespace Exato.Back.Settings;

public class DatabaseSettings
{
    public string Web { get; set; }
    public string Exato { get; set; }

    public DatabaseSettings() { }

    public DatabaseSettings(IConfiguration configuration)
    {
        configuration.GetSection("Database").Bind(this);
    }
}

public static class DatabaseSettingsExtensions
{
    extension(IConfiguration configuration)
    {
        public DatabaseSettings Database => new(configuration);
    }
}
