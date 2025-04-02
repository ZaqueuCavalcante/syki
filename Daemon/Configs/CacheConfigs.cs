using Microsoft.Extensions.Caching.Hybrid;

namespace Syki.Daemon.Configs;

public static class CacheConfigs
{
    public static void AddCacheConfigs(this WebApplicationBuilder builder)
    {
        builder.Services.AddHybridCache(options =>
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
