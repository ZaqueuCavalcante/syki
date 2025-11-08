using Exato.Workers.Events;
using Exato.Workers.Commands;

namespace Exato.Workers.Configs;

public static class HostedServicesConfigs
{
    public static void AddWorkersHostedServicesConfigs(this WebApplicationBuilder builder)
    {
        builder.Services.AddTransient<CommandsProcessor>();
        builder.Services.AddTransient<DomainEventsProcessor>();

        builder.Services.AddHostedService<NewCommandsDbListener>();
        builder.Services.AddHostedService<NewDomainEventsDbListener>();
    }
}
