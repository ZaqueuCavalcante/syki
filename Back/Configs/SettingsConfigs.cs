namespace Syki.Back.Configs;

public static class SettingsConfigs
{
    public static void AddSettingsConfigs(this WebApplicationBuilder builder)
    {
        builder.Services.AddSingleton<AuthSettings>();
        builder.Services.AddSingleton<SentrySettings>();
        builder.Services.AddSingleton<DatabaseSettings>();
        builder.Services.AddSingleton<FeaturesSettings>();
        builder.Services.AddSingleton<RateLimiterSettings>();
        builder.Services.AddSingleton<AzureBlobStorageSettings>();

        builder.Services.Configure<HostOptions>(x => x.ServicesStartConcurrently = true);
        // builder.Services.AddHostedService<LoadFeatureFlagsFromDb>();
    }
}
