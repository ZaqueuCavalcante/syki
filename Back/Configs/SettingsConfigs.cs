namespace Syki.Back.Configs;

public static class SettingsConfigs
{
    public static void AddSettingsConfigs(this WebApplicationBuilder builder)
    {
        builder.Services.AddSingleton<AuthSettings>();
        builder.Services.AddSingleton<EmailSettings>();
        builder.Services.AddSingleton<RateLimitingSettings>();
    }
}
