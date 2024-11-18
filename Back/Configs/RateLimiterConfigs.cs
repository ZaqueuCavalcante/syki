using System.Threading.RateLimiting;

namespace Syki.Back.Configs;

public static class RateLimiterConfigs
{
    public static void AddRateLimiterConfigs(this IServiceCollection services)
    {
        // TODO: Load configs from appsettings.json

        services.AddRateLimiter(options =>
        {
            options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

            if (Env.IsTesting() || Env.IsDevelopment())
            {
                options.AddFixedWindowLimiter("SuperVerySmall", o => { o.PermitLimit = 10_000; o.Window = TimeSpan.FromHours(1); });
                options.AddFixedWindowLimiter("VerySmall", o => { o.PermitLimit = 10_000; o.Window = TimeSpan.FromHours(1); });
                options.AddFixedWindowLimiter("Small", o => { o.PermitLimit = 10_000; o.Window = TimeSpan.FromHours(1); });
                options.AddFixedWindowLimiter("Medium", o => { o.PermitLimit = 10_000; o.Window = TimeSpan.FromHours(1); });
                return;
            }

            options.AddPolicy("SuperVerySmall", httpContext =>
                RateLimitPartition.GetFixedWindowLimiter(
                    partitionKey: httpContext.Connection.RemoteIpAddress?.ToString(),
                    factory: _ => new FixedWindowRateLimiterOptions
                    {
                        PermitLimit = 2,
                        Window = TimeSpan.FromHours(1)
                    }));

            options.AddPolicy("VerySmall", httpContext =>
                RateLimitPartition.GetFixedWindowLimiter(
                    partitionKey: httpContext.Connection.RemoteIpAddress?.ToString(),
                    factory: _ => new FixedWindowRateLimiterOptions
                    {
                        PermitLimit = 10,
                        Window = TimeSpan.FromHours(1)
                    }));

            options.AddPolicy("Small", httpContext =>
                RateLimitPartition.GetFixedWindowLimiter(
                    partitionKey: httpContext.Connection.RemoteIpAddress?.ToString(),
                    factory: _ => new FixedWindowRateLimiterOptions
                    {
                        PermitLimit = 20,
                        Window = TimeSpan.FromHours(1)
                    }));

            options.AddPolicy("Medium", httpContext =>
                RateLimitPartition.GetFixedWindowLimiter(
                    partitionKey: httpContext.Connection.RemoteIpAddress?.ToString(),
                    factory: _ => new FixedWindowRateLimiterOptions
                    {
                        PermitLimit = 120,
                        Window = TimeSpan.FromHours(1)
                    }));
        });
    }
}
