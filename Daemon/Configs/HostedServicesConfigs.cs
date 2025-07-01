using Syki.Daemon.Events;
using Syki.Daemon.Commands;

namespace Syki.Daemon.Configs;

public static class HostedServicesConfigs
{
    public static void AddDaemonHostedServicesConfigs(this WebApplicationBuilder builder)
    {
        builder.Services.AddTransient<CommandsProcessor>();
        builder.Services.AddTransient<DomainEventsProcessor>();

        builder.Services.AddHostedService<CommandsProcessorDbListener>();
        builder.Services.AddHostedService<DomainEventsProcessorDbListener>();
    }
}
