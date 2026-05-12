namespace Syki.Back.Settings;

public class DatabaseSettings : SettingsBase
{
    public string ConnectionString { get; set; }

    public DatabaseSettings() { }

    public DatabaseSettings(IConfiguration configuration)
    {
        configuration.GetSection("Database").Bind(this);

        RequireNonEmpty(ConnectionString);
    }
}

public static class DatabaseSettingsExtensions
{
    extension(IConfiguration configuration)
    {
        public DatabaseSettings Database => new(configuration);
    }
}
