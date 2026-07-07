using Quartz;
using Syki.Back.Webhooks;

namespace Syki.Back.Configs;

public static class QuartzConfigs
{
    public static void AddQuartzConfigs(this WebApplicationBuilder builder)
    {
        var commands = builder.Configuration.Jobs.CommandsPollingIntervalInSeconds;
        var webhookCalls = builder.Configuration.Jobs.WebhookCallsPollingIntervalInSeconds;

        builder.Services.AddQuartz(q =>
        {
            var commandsId = nameof(CommandsProcessor);
            q.AddJob<CommandsProcessor>(j => j.WithIdentity(commandsId));
            q.AddTrigger(t => t
                .ForJob(commandsId)
                .WithIdentity($"{commandsId}-trigger")
                .WithSimpleSchedule(s => s.WithIntervalInSeconds(commands).RepeatForever())
            );

            var webhookCallsId = nameof(PendingWebhookCallsProcessor);
            q.AddJob<PendingWebhookCallsProcessor>(j => j.WithIdentity(webhookCallsId));
            q.AddTrigger(t => t
                .ForJob(webhookCallsId)
                .WithIdentity($"{webhookCallsId}-trigger")
                .WithSimpleSchedule(s => s.WithIntervalInSeconds(webhookCalls).RepeatForever())
            );
        });

        builder.Services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);
    }
}
