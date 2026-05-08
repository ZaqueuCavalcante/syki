using Quartz;

namespace Syki.Back.Configs;

public static class QuartzConfigs
{
    public static void AddQuartzConfigs(this WebApplicationBuilder builder)
    {
        var commands = builder.Configuration.Jobs.CommandsPollingIntervalInSeconds;

        builder.Services.AddQuartz(q =>
        {
            var commandsId = nameof(CommandsProcessor);
            q.AddJob<CommandsProcessor>(j => j.WithIdentity(commandsId));
            q.AddTrigger(t => t
                .ForJob(commandsId)
                .WithIdentity($"{commandsId}-trigger")
                .WithSimpleSchedule(s => s.WithIntervalInSeconds(commands).RepeatForever())
            );
        });

        builder.Services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);
    }
}
