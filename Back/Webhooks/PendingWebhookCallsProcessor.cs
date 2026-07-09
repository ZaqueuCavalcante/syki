using Quartz;
using System.Diagnostics;
using Estud.Back.Features.Webhooks.CallWebhooks;

namespace Estud.Back.Webhooks;

public class PendingWebhookCallsProcessor(IServiceScopeFactory serviceScopeFactory) : IJob
{
    private static readonly ActivitySource _activitySource = new(OpenTelemetryConfigs.WebhookCallsProcessing);

    public async Task Execute(IJobExecutionContext context)
    {
        using var scope = serviceScopeFactory.CreateScope();
        var ctx = scope.ServiceProvider.GetRequiredService<EstudDbContext>();

        await Process(scope, ctx);
    }

    private static async Task Process(IServiceScope scope, EstudDbContext ctx)
    {
        while (true)
        {
            using var activity = _activitySource.StartActivity("Enqueue CallWebhookCommands", ActivityKind.Producer);

            await ctx.Database.BeginTransactionAsync();

            var calls = await ctx.WebhookCalls.FromSqlRaw(Sql).AsNoTracking().ToListAsync();
            if (calls.Count == 0)
            {
                await ctx.Database.RollbackTransactionAsync();
                break;
            }

            activity?.SetTag("webhook_calls.count", calls.Count);

            foreach (var call in calls)
            {
                ctx.AddCommand(call.InstitutionId, new CallWebhookCommand(call.Id));
            }

            await ctx.SaveChangesAsync();
            await ctx.Database.CommitTransactionAsync();

            var scheduler = await scope.ServiceProvider.GetRequiredService<ISchedulerFactory>().GetScheduler();
            await scheduler.TriggerCommandsProcessorJob();
        }
    }

    /// <summary>
    /// See <see cref="WebhookCallStatus"/> for status mapping
    /// </summary>
    private static readonly string Sql = @"
        UPDATE estud.webhook_calls
        SET status = 1
        WHERE ctid IN (
            SELECT ctid
            FROM estud.webhook_calls
            WHERE status = 0
            ORDER BY created_at
            FOR UPDATE SKIP LOCKED
            LIMIT 100
        )
        RETURNING *;
    ";
}
