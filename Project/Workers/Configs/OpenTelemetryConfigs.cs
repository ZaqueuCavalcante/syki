using Npgsql;
using OpenTelemetry.Trace;
using Exato.Back.Settings;
using OpenTelemetry.Resources;

namespace Exato.Workers.Configs;

public static class OpenTelemetryConfigs
{
    public const string DomainEventsProcessing =  nameof(DomainEventsProcessing);
    public const string CommandsProcessing = nameof(CommandsProcessing);

    public static void AddWorkersOpenTelemetryConfigs(this WebApplicationBuilder builder)
    {
        var settings = builder.Configuration.OpenTelemetry;

        if (!settings.Enabled) return;

        builder.Services
            .AddOpenTelemetry()
            .ConfigureResource(resource => resource.AddService("Workers"))
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
                    .SetSampler(new TraceIdRatioBasedSampler(settings.TracingSamplingRatio))
                    .AddOtlpExporter();
            });
    }
}
