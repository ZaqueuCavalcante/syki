namespace Syki.Daemon.Configs;

public static class SettingsConfigs
{
    public static void AddDaemonSettingsConfigs(this WebApplicationBuilder builder)
    {
        builder.Services.AddSingleton<EmailSettings>();
        builder.Services.AddSingleton<DatabaseSettings>();
    }
}
