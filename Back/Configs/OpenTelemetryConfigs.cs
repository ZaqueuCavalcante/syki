using Npgsql;
using OpenTelemetry.Trace;
using OpenTelemetry.Resources;

namespace Syki.Back.Configs;

public static class OpenTelemetryConfigs
{
    public static void AddOpenTelemetryConfigs(this IServiceCollection services)
    {
        services
            .AddOpenTelemetry()
            .ConfigureResource(resource => resource.AddService("syki-api"))
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
