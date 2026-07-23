using Quartz;
using System.Diagnostics;
using System.Collections.Concurrent;
using Estud.Back.Domain.DomainEvents;

namespace Estud.Back.DomainEvents;

public class DomainEventsProcessor(IServiceScopeFactory serviceScopeFactory) : IJob
{
    private static readonly ActivitySource _activitySource = new (OpenTelemetryConfigs.DomainEventsProcessing);

    public async Task Execute(IJobExecutionContext context)
    {
        using var scope = serviceScopeFactory.CreateScope();
        var ctx = scope.ServiceProvider.GetRequiredService<EstudDbContext>();

        await Process(scope, ctx, Guid.CreateVersion7());
    }

    private static async Task Process(IServiceScope scope, EstudDbContext ctx, Guid processorId)
    {
        var sw = Stopwatch.StartNew();

        while (true)
        {
            var events = await ctx.DomainEvents.FromSqlRaw(Sql, processorId).AsNoTracking().ToListAsync();
            if (events.Count == 0) break;

            foreach (var evt in events)
            {
                using (var activity = _activitySource.StartActivity($"Process {evt.Type.Split('.').Last()}", ActivityKind.Consumer, evt.GetParentContext()))
                {
                    activity?.SetTag("event.id", evt.Id);
                    activity?.SetTag("event.type", evt.Type.Split('.').Last());
                    activity?.SetTag("event.institutionId", evt.InstitutionId);
                    ctx.ActivityId = evt.ActivityId!;
                    ctx.Operation = evt.Type;
                    ctx.RequestUser.InstitutionId = evt.InstitutionId;

                    try
                    {
                        sw.Restart();

                        ctx.ChangeTracker.Clear();
                        ctx.Attach(evt);

                        await ctx.Database.BeginTransactionAsync();
                        ctx.Database.AutoSavepointsEnabled = false;

                        var invoker = GetInvoker(evt);
                        await invoker.Invoke(scope.ServiceProvider, evt.InstitutionId, evt.Id, evt.Data);
                    }
                    catch (Exception ex)
                    {
                        if (ctx.Database.CurrentTransaction is not null)
                            await ctx.Database.RollbackTransactionAsync();

                        ctx.ChangeTracker.Clear();
                        ctx.Attach(evt);
                        evt.Error = ex.Message + ex.InnerException?.Message;
                        activity?.SetStatus(ActivityStatusCode.Error, ex.Message);
                        activity?.AddException(ex);
                        await ctx.Database.BeginTransactionAsync();
                    }

                    sw.Stop();
                    evt.Processed(sw.Elapsed.TotalMilliseconds);
                    activity?.SetTag("event.duration", evt.Duration);

                    await ctx.SaveChangesAsync();
                    await ctx.Database.CommitTransactionAsync();
                }
            }

            ctx.ChangeTracker.Clear();

            // Eventos criados pelos handlers são drenados pelo próprio loop.
            // Commands criados por eles não, então o processor deles é disparado aqui.
            if (ctx.HasPendingCommands)
            {
                var scheduler = await scope.ServiceProvider.GetRequiredService<ISchedulerFactory>().GetScheduler();
                await scheduler.TriggerCommandsProcessorJob();
                ctx.HasPendingCommands = false;
            }
        }
    }

    private static readonly ConcurrentDictionary<string, IDomainEventInvoker> _invokers = new();
    private static IDomainEventInvoker GetInvoker(DomainEvent evt)
    {
        return _invokers.GetOrAdd(evt.Type, typeName =>
        {
            var eventType = DomainEventConfigs.GetDomainEventType(typeName);
            var invokerType = typeof(DomainEventInvoker<>).MakeGenericType(eventType);
            return (IDomainEventInvoker)Activator.CreateInstance(invokerType)!;
        });
    }

    /// <summary>
    /// See <see cref="DomainEventStatus"/> for status mapping
    /// </summary>
    private static readonly string Sql = @"
        UPDATE estud.domain_events
        SET processor_id = {0}, status = 1
        WHERE ctid IN (
            SELECT ctid
            FROM estud.domain_events
            WHERE processor_id IS NULL AND status = 0
            ORDER BY occurred_at
            FOR UPDATE SKIP LOCKED
            LIMIT 100
        )
        RETURNING *;
    ";
}
