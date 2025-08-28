using Npgsql;
using OpenTelemetry.Logs;
using OpenTelemetry.Trace;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;

namespace Syki.Back.Configs;

public static class OpenTelemetryConfigs
{
    public static void AddOpenTelemetryConfigs(this WebApplicationBuilder builder)
    {
        if (Env.IsTesting()) return;

        // builder.Logging.AddOpenTelemetry(logging =>
        // {
        //     logging.IncludeScopes = true;
        //     logging.IncludeFormattedMessage = true;
        // });

        var settings = builder.Configuration.Tracing();

        builder.Services
            .AddOpenTelemetry()
            .ConfigureResource(resource => resource.AddService("Back"))
            // .WithMetrics(metrics =>
            // {
            //     metrics
            //         .AddNpgsqlInstrumentation()
            //         .AddRuntimeInstrumentation()
            //         .AddAspNetCoreInstrumentation()
            //         .AddHttpClientInstrumentation();

            //     metrics.AddMeter("Microsoft.AspNetCore.Hosting");
            //     metrics.AddMeter("Microsoft.AspNetCore.Server.Kestrel");

            //     metrics.AddOtlpExporter();
            // })
            .WithTracing(tracing =>
            {
                tracing
                    .AddNpgsql()
                    .AddQuartzInstrumentation()
                    .AddHttpClientInstrumentation()
                    .AddAspNetCoreInstrumentation();

                tracing
                    .SetSampler(new TraceIdRatioBasedSampler(settings.SamplingRatio))
                    .AddOtlpExporter();
            });
    }
}
