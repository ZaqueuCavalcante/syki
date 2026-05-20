using Quartz;
using System.Diagnostics;
using Syki.Back.Domain.Enums;
using System.Collections.Concurrent;
using Syki.Back.Commands.Domain.Commands;

namespace Syki.Back.Commands;

public class CommandsProcessor(IServiceScopeFactory serviceScopeFactory) : IJob
{
    private static readonly ActivitySource _activitySource = new (OpenTelemetryConfigs.CommandsProcessing);

    public async Task Execute(IJobExecutionContext context)
    {
        using var scope = serviceScopeFactory.CreateScope();
        var ctx = scope.ServiceProvider.GetRequiredService<SykiDbContext>();

        await Process(scope, ctx);
    }

    private static async Task Process(IServiceScope scope, SykiDbContext ctx)
    {
        var processorId = Guid.NewGuid();
        var sw = Stopwatch.StartNew();

        while (true)
        {
            var commands = await ctx.Commands.FromSqlRaw(Sql, processorId).AsNoTracking().ToListAsync();
            if (commands.Count == 0) break;

            foreach (var command in commands)
            {
                using (var activity = _activitySource.StartActivity($"Process {command.Type}", ActivityKind.Consumer, command.GetParentActivityContext()))
                {
                    activity?.SetTag("command.id", command.Id);
                    activity?.SetTag("command.type", command.Type);
                    ctx.ActivityId = command.ActivityId!;
                    ctx.Operation = command.Type;
                    ctx.CommandLogs = [];

                    try
                    {
                        sw.Restart();

                        ctx.ChangeTracker.Clear();
                        ctx.Attach(command);

                        await ctx.Database.BeginTransactionAsync();
                        ctx.Database.AutoSavepointsEnabled = false;

                        var invoker = GetInvoker(command);
                        await invoker.Invoke(scope.ServiceProvider, command.Id, command.Data);
                    }
                    catch (Exception ex)
                    {
                        if (ctx.Database.CurrentTransaction is not null)
                            await ctx.Database.RollbackTransactionAsync();

                        ctx.ChangeTracker.Clear();
                        ctx.Attach(command);
                        command.Error = ex.Message + ex.InnerException?.Message;
                        activity?.SetStatus(ActivityStatusCode.Error, ex.Message);
                        activity?.AddException(ex);
                        await ctx.Database.BeginTransactionAsync();
                    }

                    sw.Stop();
                    command.Logs = ctx.CommandLogs;
                    command.Processed(sw.Elapsed.TotalMilliseconds);
                    activity?.SetTag("command.duration", command.Duration);

                    if (command.Error.HasValue() && command.MaxRetries > 0)
                    {
                        var retryCommand = CreateRetryCommand(command, ctx.ActivityId);
                        ctx.Commands.Add(retryCommand);
                    }

                    await ctx.SaveChangesAsync();
                    await ctx.Database.CommitTransactionAsync();
                }
            }

            if (ctx.HasPendingCommands)
            {
                var scheduler = await scope.ServiceProvider.GetRequiredService<ISchedulerFactory>().GetScheduler();
                await scheduler.TriggerCommandsProcessorJob();
            }
        }
    }

    private static Command CreateRetryCommand(Command failedCommand, string activityId)
    {
        var invoker = GetInvoker(failedCommand);
        var data = invoker.Deserialize(failedCommand.Data);

        var originalId = failedCommand.OriginalId ?? failedCommand.Id;

        var retryAttempt = failedCommand.RetryAttempt + 1;
        var delaySeconds = CommandBackoffStrategies.GetDelaySeconds(
            failedCommand.BackoffStrategy, failedCommand.BaseDelaySeconds, retryAttempt);

        var retryCommand = new Command(
            failedCommand.InstitutionId,
            data,
            originalId: originalId,
            activityId: activityId,
            maxRetries: failedCommand.MaxRetries - 1,
            delaySeconds: delaySeconds,
            backoffStrategy: failedCommand.BackoffStrategy,
            baseDelaySeconds: failedCommand.BaseDelaySeconds)
        {
            Type = failedCommand.Type,
            RetryAttempt = retryAttempt,
        };

        return retryCommand;
    }

    private static readonly ConcurrentDictionary<string, ICommandInvoker> _invokers = new();
    private static ICommandInvoker GetInvoker(Command command)
    {
        return _invokers.GetOrAdd(command.Type, typeName =>
        {
            var commandType = CommandConfigs.GetCommandType(typeName);
            var invokerType = typeof(CommandInvoker<>).MakeGenericType(commandType);
            return (ICommandInvoker)Activator.CreateInstance(invokerType)!;
        });
    }

    /// <summary>
    /// See <see cref="CommandStatus"/> for status mapping
    /// </summary>
    private static readonly string Sql = @"
        UPDATE syki.commands
        SET processor_id = {0}, status = 2
        WHERE ctid IN (
            SELECT ctid
            FROM syki.commands
            WHERE processor_id IS NULL AND status = 0 AND (not_before IS NULL OR not_before < NOW())
            ORDER BY created_at
            FOR UPDATE SKIP LOCKED
            LIMIT 10
        )
        RETURNING *;
    ";
}
