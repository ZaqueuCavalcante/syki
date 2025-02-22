using System.Threading.RateLimiting;

namespace Syki.Back.Configs;

public static class RateLimiterConfigs
{
    public static void AddRateLimiterConfigs(this IServiceCollection services)
    {
        using var serviceProvider = services.BuildServiceProvider();
        var settings = serviceProvider.GetService<RateLimiterSettings>()!;

        services.AddRateLimiter(options =>
        {
            options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

            options.AddPolicy(nameof(settings.SuperVerySmall), httpContext =>
                RateLimitPartition.GetFixedWindowLimiter(
                    partitionKey: httpContext.Connection.RemoteIpAddress?.ToString(),
                    factory: _ => new FixedWindowRateLimiterOptions
                    {
                        PermitLimit = settings.SuperVerySmall,
                        Window = TimeSpan.FromHours(1)
                    }));

            options.AddPolicy(nameof(settings.VerySmall), httpContext =>
                RateLimitPartition.GetFixedWindowLimiter(
                    partitionKey: httpContext.Connection.RemoteIpAddress?.ToString(),
                    factory: _ => new FixedWindowRateLimiterOptions
                    {
                        PermitLimit = settings.VerySmall,
                        Window = TimeSpan.FromHours(1)
                    }));

            options.AddPolicy(nameof(settings.Small), httpContext =>
                RateLimitPartition.GetFixedWindowLimiter(
                    partitionKey: httpContext.Connection.RemoteIpAddress?.ToString(),
                    factory: _ => new FixedWindowRateLimiterOptions
                    {
                        PermitLimit = settings.Small,
                        Window = TimeSpan.FromHours(1)
                    }));

            options.AddPolicy(nameof(settings.Medium), httpContext =>
                RateLimitPartition.GetFixedWindowLimiter(
                    partitionKey: httpContext.Connection.RemoteIpAddress?.ToString(),
                    factory: _ => new FixedWindowRateLimiterOptions
                    {
                        PermitLimit = settings.Medium,
                        Window = TimeSpan.FromHours(1)
                    }));
        });
    }
}
