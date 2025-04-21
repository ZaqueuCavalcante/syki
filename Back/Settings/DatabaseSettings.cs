namespace Syki.Back.Settings;

public class DatabaseSettings
{
    public bool Reset { get; set; }
    public string ConnectionString { get; set; }

    public DatabaseSettings() { }

    public DatabaseSettings(IConfiguration configuration)
    {
        configuration.GetSection("Database").Bind(this);
    }
}
