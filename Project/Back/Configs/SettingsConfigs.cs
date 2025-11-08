namespace Exato.Back.Configs;

public static class SettingsConfigs
{
    public static void AddSettingsConfigs(this WebApplicationBuilder builder)
    {
        builder.Services.AddSingleton<AuthSettings>();
        builder.Services.AddSingleton<IntelligenceApiSettings>();
    }
}
