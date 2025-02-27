namespace Syki.Back.Configs;

public static class CacheConfigs
{
    public static void AddCacheConfigs(this IServiceCollection services)
    {
        services.AddHybridCache(options =>
        {
            options.MaximumKeyLength = 512;
            options.MaximumPayloadBytes = 10 * 1024 * 1024;

            options.DefaultEntryOptions = new HybridCacheEntryOptions
            {
                Expiration = TimeSpan.FromMinutes(30),
                LocalCacheExpiration = TimeSpan.FromMinutes(30),
            };
        });
    }
}
