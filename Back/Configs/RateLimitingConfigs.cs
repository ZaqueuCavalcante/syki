using System.Text.Json;
using System.Threading.RateLimiting;

namespace Syki.Back.Configs;

public static class RateLimitingConfigs
{
    public const string SensitivePolicy = nameof(SensitivePolicy);

    public static void AddRateLimitingConfigs(this WebApplicationBuilder builder)
    {
        var settings = builder.Configuration.RateLimiting;

        builder.Services.AddRateLimiter(options =>
        {
            options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(context =>
            {
                var userId = context.User?.Id;
                var partitionKey = userId != null
                    ? $"user_{userId}"
                    : $"ip_{context.Connection.RemoteIpAddress}";

                return RateLimitPartition.GetFixedWindowLimiter(partitionKey, _ => new FixedWindowRateLimiterOptions
                {
                    QueueLimit = settings.QueueLimit,
                    PermitLimit = settings.GlobalPermitLimit,
                    Window = TimeSpan.FromSeconds(settings.GlobalWindowInSeconds),
                });
            });

            options.AddPolicy(SensitivePolicy, context =>
            {
                var partitionKey = $"ip_{context.Connection.RemoteIpAddress}";

                return RateLimitPartition.GetFixedWindowLimiter(partitionKey, _ => new FixedWindowRateLimiterOptions
                {
                    QueueLimit = settings.QueueLimit,
                    PermitLimit = settings.SensitivePermitLimit,
                    Window = TimeSpan.FromSeconds(settings.SensitiveWindowInSeconds),
                });
            });

            options.OnRejected = async (context, cancellationToken) =>
            {
                context.HttpContext.Response.ContentType = "application/json";
                context.HttpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;

                if (context.Lease.TryGetMetadata(MetadataName.RetryAfter, out var retryAfter))
                {
                    context.HttpContext.Response.Headers.RetryAfter = ((int)retryAfter.TotalSeconds).ToString();
                }

                await context.HttpContext.Response.WriteAsync(JsonSerializer.Serialize(TooManyRequests.I), cancellationToken);
            };
        });
    }

    public static void UseRateLimiting(this IApplicationBuilder app)
    {
        app.UseRateLimiter();
    }
}
