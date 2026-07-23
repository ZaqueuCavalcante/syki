using Quartz;
using Estud.Back.Webhooks;

namespace Estud.Tests.Base;

public static class BackFactoryBackground
{
    public static async Task AwaitCommandsProcessing(this BackFactory factory)
    {
        await using var ctx = factory.GetDbContext();

        var scheduler = await factory.GetSchedulerFactory().GetScheduler();
        await scheduler.TriggerCommandsProcessorJob();

        var count = 0;
        while (true)
        {
            if (count == 5) break;

            var commands = await ctx.Commands.CountAsync(x => x.ProcessedAt == null);
            if (commands == 0) break;
            await Task.Delay(500);
            count ++;
        }
    }

    public static async Task AwaitDomainEventsProcessing(this BackFactory factory)
    {
        await using var ctx = factory.GetDbContext();

        var scheduler = await factory.GetSchedulerFactory().GetScheduler();
        await scheduler.TriggerDomainEventsProcessorJob();

        var count = 0;
        while (true)
        {
            if (count == 5) break;

            var events = await ctx.DomainEvents.CountAsync(x => x.ProcessedAt == null);
            if (events == 0) break;
            await Task.Delay(500);
            count ++;
        }

        await factory.AwaitCommandsProcessing();
    }

    public static async Task AwaitWebhookCallsProcessing(this BackFactory factory)
    {
        await using var ctx = factory.GetDbContext();

        var scheduler = await factory.GetSchedulerFactory().GetScheduler();
        await scheduler.TriggerJob(new JobKey(nameof(PendingWebhookCallsProcessor)));

        var count = 0;
        while (true)
        {
            if (count == 10) break;

            var pending = await ctx.WebhookCalls.CountAsync(x => x.Status == WebhookCallStatus.Pending);
            if (pending == 0) break;
            await Task.Delay(500);
            count++;
        }

        await factory.AwaitCommandsProcessing();
    }
}
