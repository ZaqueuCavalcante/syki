namespace Syki.Back.Configs;

public static class SettingsConfigs
{
    public static void AddSettingsConfigs(this IServiceCollection services)
    {
        services.AddSingleton<AuthSettings>();
        services.AddSingleton<DatabaseSettings>();
        services.AddSingleton<FeaturesSettings>();
        services.AddSingleton<RateLimiterSettings>();

        services.Configure<HostOptions>(x => x.ServicesStartConcurrently = true);
        services.AddHostedService<LoadFeatureFlagsFromDb>();
    }
}
