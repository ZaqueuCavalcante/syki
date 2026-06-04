using Quartz;
using System.Diagnostics;
using System.Collections.Concurrent;

namespace Syki.Back.Webhooks;

public class ReceivedWebhookEventsProcessor(IServiceScopeFactory serviceScopeFactory) : IJob
{
    private static readonly ActivitySource _activitySource = new(OpenTelemetryConfigs.WebhookEventsProcessing);

    public async Task Execute(IJobExecutionContext context)
    {
        using var scope = serviceScopeFactory.CreateScope();
        var ctx = scope.ServiceProvider.GetRequiredService<SykiDbContext>();

        await Process(scope, ctx);
    }

    private static async Task Process(IServiceScope scope, SykiDbContext ctx)
    {
        var processorId = Guid.NewGuid();
        var commandsCreated = false;

        while (true)
        {
            var events = await ctx.ReceivedWebhookEvents.FromSqlRaw(Sql, processorId).ToListAsync();
            if (events.Count == 0) break;

            foreach (var evt in events)
            {
                using var activity = _activitySource.StartActivity($"Process {evt.Type}", ActivityKind.Consumer);
                activity?.SetTag("webhook_event.id", evt.Id);
                activity?.SetTag("webhook_event.type", evt.Type);
                activity?.SetTag("webhook_event.source", evt.Source.ToString());

                try
                {
                    ctx.ChangeTracker.Clear();
                    ctx.Attach(evt);

                    await ctx.Database.BeginTransactionAsync();

                    if (WebhookConfigs.TryGetEventType(evt.Type, out _))
                    {
                        var invoker = GetInvoker(evt.Type);
                        await invoker.Invoke(scope.ServiceProvider, evt);
                        commandsCreated = commandsCreated || evt.CommandId != null;
                    }
                    else
                    {
                        evt.Status = ReceivedWebhookEventStatus.Ignored;
                        evt.ProcessedAt = DateTime.Now;
                    }

                    await ctx.SaveChangesAsync();
                    await ctx.Database.CommitTransactionAsync();
                }
                catch (Exception ex)
                {
                    if (ctx.Database.CurrentTransaction is not null)
                        await ctx.Database.RollbackTransactionAsync();

                    ctx.ChangeTracker.Clear();
                    ctx.Attach(evt);
                    evt.Status = ReceivedWebhookEventStatus.Error;
                    evt.Error = ex.Message + ex.InnerException?.Message;
                    evt.ProcessedAt = DateTime.Now;
                    await ctx.SaveChangesAsync();

                    activity?.SetStatus(ActivityStatusCode.Error, ex.Message);
                    activity?.AddException(ex);
                }
            }
        }

        if (commandsCreated)
        {
            var scheduler = await scope.ServiceProvider.GetRequiredService<ISchedulerFactory>().GetScheduler();
            await scheduler.TriggerCommandsProcessorJob();
        }
    }

    private static readonly ConcurrentDictionary<string, IWebhookEventInvoker> _invokers = new();
    private static IWebhookEventInvoker GetInvoker(string eventType)
    {
        return _invokers.GetOrAdd(eventType, type =>
        {
            WebhookConfigs.TryGetEventType(type, out var webhookEventType);
            var invokerType = typeof(WebhookEventInvoker<>).MakeGenericType(webhookEventType);
            return (IWebhookEventInvoker)Activator.CreateInstance(invokerType)!;
        });
    }

    /// <summary>
    /// See <see cref="ReceivedWebhookEventStatus"/> for status mapping
    /// </summary>
    private static readonly string Sql = @"
        UPDATE web.received_webhook_events
        SET processor_id = {0}, status = 1
        WHERE ctid IN (
            SELECT ctid
            FROM web.received_webhook_events
            WHERE processor_id IS NULL AND status = 0
            ORDER BY created_at
            FOR UPDATE SKIP LOCKED
            LIMIT 10
        )
        RETURNING *;
    ";
}
