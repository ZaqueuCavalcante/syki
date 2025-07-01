using Newtonsoft.Json;
using Syki.Back.Events;
using System.Diagnostics;
using Syki.Back.Database;
using Syki.Daemon.Configs;
using System.Collections.Concurrent;

namespace Syki.Daemon.Events;

public class DomainEventsProcessor(IServiceScopeFactory serviceScopeFactory)
{
    private static readonly ActivitySource _activitySource = new (OpenTelemetryConfigs.DomainEventsProcessing);

    public async Task Run()
    {
        using var scope = serviceScopeFactory.CreateScope();
        var ctx = scope.ServiceProvider.GetRequiredService<SykiDbContext>();

        await Process(scope, ctx, Guid.CreateVersion7());
    }

    private static async Task Process(IServiceScope scope, SykiDbContext ctx, Guid processorId)
    {
        var events = await ctx.DomainEvents.FromSqlRaw(Sql, processorId).ToListAsync();
        if (events.Count == 0) return;

        await ctx.Database.BeginTransactionAsync();
        ctx.Database.AutoSavepointsEnabled = false;
        var sw = Stopwatch.StartNew();

        foreach (var evt in events)
        {
            using (var activity = _activitySource.StartActivity($"Process {evt.Type.Split('.').Last()}", ActivityKind.Consumer, evt.GetParentContext()))
            {
                activity?.SetTag("event.id", evt.Id);
                activity?.SetTag("event.type", evt.Type.Split('.').Last());
                activity?.SetTag("event.institutionId", evt.InstitutionId);

                try
                {
                    sw.Restart();
                    dynamic data = GetData(evt);
                    dynamic handler = GetHandler(scope, evt);
                    await handler.Handle(evt.InstitutionId, evt.Id, data);
                }
                catch (Exception ex)
                {
                    evt.Error = ex.Message + ex.InnerException?.Message;
                    activity?.SetStatus(ActivityStatusCode.Error, ex.Message);
                    activity?.AddException(ex);
                }
                finally
                {
                    sw.Stop();
                    evt.Processed(sw.Elapsed.TotalMilliseconds);
                    activity?.SetTag("event.duration", evt.Duration);
                }
            }
        }

        await ctx.SaveChangesAsync();
        await ctx.Database.CommitTransactionAsync();

        ctx.ChangeTracker.Clear();

        await Process(scope, ctx, processorId);
    }

    private static readonly ConcurrentDictionary<string, Type> _types = new();
    private static dynamic GetData(DomainEvent evt)
    {
        var type = _types.GetOrAdd(evt.Type, typeof(DomainEvent).Assembly.GetType(evt.Type)!);
        dynamic data = JsonConvert.DeserializeObject(evt.Data, type)!;
        return data;
    }

    private static readonly ConcurrentDictionary<string, Type> _handlers = new();
    private static dynamic GetHandler(IServiceScope scope, DomainEvent evt)
    {
        var handlerType = _handlers.GetOrAdd(evt.Type, typeof(IDomainEvent).Assembly.GetType($"{evt.Type}Handler")!);
        dynamic handler = scope.ServiceProvider.GetRequiredService(handlerType);
        return handler;
    }

    private static readonly string Sql = @"
        UPDATE syki.domain_events
        SET processor_id = {0}, status = 'Processing'
        WHERE ctid IN (
            SELECT ctid
            FROM syki.domain_events
            WHERE processor_id IS NULL
            ORDER BY occurred_at
            FOR UPDATE SKIP LOCKED
            LIMIT 100
        )
        RETURNING *;
    ";
}
