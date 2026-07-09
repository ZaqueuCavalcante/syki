using Npgsql;
using OpenTelemetry.Trace;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;

namespace Estud.Back.Configs;

public static class OpenTelemetryConfigs
{
    public const string CommandsProcessing = nameof(CommandsProcessing);
    public const string WebhookCallsProcessing = nameof(WebhookCallsProcessing);
    public const string WebhookEventsProcessing = nameof(WebhookEventsProcessing);

    public static void AddOpenTelemetryConfigs(this WebApplicationBuilder builder)
    {
        var settings = builder.Configuration.OpenTelemetry;

        if (!settings.Enabled) return;

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

                metrics
                    .AddMeter("Microsoft.AspNetCore.Hosting")
                    .AddMeter("Microsoft.AspNetCore.Server.Kestrel")
                    .AddOtlpExporter();
            })
            .WithTracing(tracing =>
            {
                tracing
                    .AddNpgsql()
                    .AddHttpClientInstrumentation()
                    .AddAspNetCoreInstrumentation();

                tracing
                    .SetSampler(new TraceIdRatioBasedSampler(settings.TracingSamplingRatio))
                    .AddOtlpExporter();
            });
    }
}
