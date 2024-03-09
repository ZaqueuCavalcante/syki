using System.Threading.RateLimiting;

namespace Syki.Back.Configs;

public static class RateLimiterConfigs
{
    public static void AddRateLimiterConfigs(this IServiceCollection services)
    {
        services.AddRateLimiter(options =>
        {
            options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

            if (Env.IsTesting() || Env.IsDevelopment())
            {
                options.AddFixedWindowLimiter("VerySmall", o => { o.PermitLimit = 1000; o.Window = TimeSpan.FromHours(1); });
                options.AddFixedWindowLimiter("Small", o => { o.PermitLimit = 1000; o.Window = TimeSpan.FromHours(1); });
                options.AddFixedWindowLimiter("Medium", o => { o.PermitLimit = 1000; o.Window = TimeSpan.FromHours(1); });
                return;
            }

            options.AddPolicy("VerySmall", httpContext =>
                RateLimitPartition.GetFixedWindowLimiter(
                    partitionKey: httpContext.Connection.RemoteIpAddress?.ToString(),
                    factory: _ => new FixedWindowRateLimiterOptions
                    {
                        PermitLimit = 2,
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
