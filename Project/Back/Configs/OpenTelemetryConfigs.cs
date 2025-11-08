using Npgsql;
using OpenTelemetry.Trace;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;

namespace Exato.Back.Configs;

public static class OpenTelemetryConfigs
{
    public static void AddOpenTelemetryConfigs(this WebApplicationBuilder builder)
    {
        var settings = builder.Configuration.OpenTelemetry;

        if (!settings.Enabled) return;

        // TODO: Implement this
        // builder.Logging.AddOpenTelemetry(logging =>
        // {
        //     logging.IncludeScopes = true;
        //     logging.IncludeFormattedMessage = true;
        // });

        builder.Services
            .AddOpenTelemetry()
            .ConfigureResource(resource => resource.AddService("Back"))
            .WithMetrics(metrics =>
            {
                metrics
                    .AddNpgsqlInstrumentation()
                    .AddRuntimeInstrumentation()
                    .AddAspNetCoreInstrumentation()
                    .AddHttpClientInstrumentation();

                metrics.AddMeter("Microsoft.AspNetCore.Hosting");
                metrics.AddMeter("Microsoft.AspNetCore.Server.Kestrel");

                metrics.AddOtlpExporter();
            })
            .WithTracing(tracing =>
            {
                tracing
                    .AddNpgsql()
                    .AddQuartzInstrumentation()
                    .AddHttpClientInstrumentation()
                    .AddAspNetCoreInstrumentation();

                tracing
                    .SetSampler(new TraceIdRatioBasedSampler(settings.TracingSamplingRatio))
                    .AddOtlpExporter();
            });
    }
}
