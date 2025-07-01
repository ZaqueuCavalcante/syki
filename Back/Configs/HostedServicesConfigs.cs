using Syki.Back.Metrics;
using Syki.Back.Features.Cross.LoadFeatureFlags;

namespace Syki.Back.Configs;

public static class HostedServicesConfigs
{
    public static void AddHostedServicesConfigs(this WebApplicationBuilder builder)
    {
        builder.Services.Configure<HostOptions>(x => x.ServicesStartConcurrently = true);

        builder.Services.AddHostedService<SetupCurrentMetricsStore>();
        builder.Services.AddHostedService<LoadFeatureFlagsHostedService>();
    }
}
