using Npgsql;
using OpenTelemetry.Trace;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;

namespace Syki.Back.Configs;

public static class OpenTelemetryConfigs
{
    public static void AddOpenTelemetryConfigs(this IServiceCollection services)
    {
        if (Env.IsTesting()) return;

        services
            .AddOpenTelemetry()
            .ConfigureResource(resource => resource.AddService("SykiAPI"))
            .WithMetrics(metrics =>
            {
                metrics
                    .AddAspNetCoreInstrumentation()
                    .AddNpgsqlInstrumentation();
                
                metrics.AddOtlpExporter();
            })
            .WithTracing(tracing =>
            {
                tracing
                    .AddAspNetCoreInstrumentation()
                    .AddEntityFrameworkCoreInstrumentation()
                    .AddNpgsql();

                tracing.AddOtlpExporter();
            });
    }
}
