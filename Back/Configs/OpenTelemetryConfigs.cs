using Npgsql;
using OpenTelemetry.Logs;
using OpenTelemetry.Trace;
using Sentry.OpenTelemetry;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;

namespace Syki.Back.Configs;

public static class OpenTelemetryConfigs
{
    public static void AddOpenTelemetryConfigs(this WebApplicationBuilder builder)
    {
        if (Env.IsTesting()) return;

        var settins = new SentrySettings(builder.Configuration);

        builder.Services
            .AddOpenTelemetry()
            .ConfigureResource(resource => resource.AddService("SykiAPI"))
            .WithMetrics(metrics =>
            {
                metrics
                    .AddNpgsqlInstrumentation()
                    .AddRuntimeInstrumentation()
                    .AddAspNetCoreInstrumentation();

                metrics.AddOtlpExporter();
            })
            .WithTracing(tracing =>
            {
                tracing
                    .AddSentry()
                    .AddNpgsql()
                    .AddAspNetCoreInstrumentation()
                    .AddEntityFrameworkCoreInstrumentation();

                tracing.AddOtlpExporter();
            })
            .WithLogging(logging =>
            {
                logging.AddOtlpExporter();
            });

        builder.WebHost.UseSentry(options =>
        {
            options.Dsn = settins.Dsn;
            options.UseOpenTelemetry();
            options.SendDefaultPii = true;
            options.TracesSampleRate = settins.TracesSampleRate;
        });
    }
}
