using Hangfire;
using Syki.Daemon.Events;
using Syki.Daemon.Commands;

namespace Syki.Daemon.Startup;

public class EnqueueProcessors : IHostedService
{
    public Task StartAsync(CancellationToken cancellationToken)
    {
        BackgroundJob.Enqueue<CommandsProcessor>(x => x.Run());
        BackgroundJob.Enqueue<DomainEventsProcessor>(x => x.Run());
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
