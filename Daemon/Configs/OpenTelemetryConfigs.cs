using Npgsql;
using OpenTelemetry.Logs;
using OpenTelemetry.Trace;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;

namespace Syki.Daemon.Configs;

public static class OpenTelemetryConfigs
{
    public const string DomainEventsProcessing =  nameof(DomainEventsProcessing);
    public const string CommandsProcessing = nameof(CommandsProcessing);

    public static void AddDaemonOpenTelemetryConfigs(this WebApplicationBuilder builder)
    {
        if (Env.IsTesting()) return;

        var settings = builder.Configuration.Tracing();

        builder.Services
            .AddOpenTelemetry()
            .ConfigureResource(resource => resource.AddService("Daemon"))
            .WithTracing(tracing =>
            {
                tracing.AddSource(DomainEventsProcessing);
                tracing.AddSource(CommandsProcessing);

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
