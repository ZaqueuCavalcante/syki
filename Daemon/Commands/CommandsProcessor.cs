using Newtonsoft.Json;
using System.Diagnostics;
using Syki.Back.Database;
using System.Collections.Concurrent;

namespace Syki.Daemon.Commands;

public class CommandsProcessor(IServiceScopeFactory serviceScopeFactory)
{
    public async Task Run()
    {
        using var scope = serviceScopeFactory.CreateScope();
        var ctx = scope.ServiceProvider.GetRequiredService<SykiDbContext>();

        await Process(scope, ctx, Guid.CreateVersion7());
    }

    private static async Task Process(IServiceScope scope, SykiDbContext ctx, Guid processorId)
    {
        var commands = await ctx.Commands.FromSqlRaw(Sql, processorId).AsNoTracking().ToListAsync();
        if (commands.Count == 0) return;

        var sw = Stopwatch.StartNew();

        foreach (var command in commands)
        {
            sw.Restart();

            ctx.Attach(command);
            await ctx.Database.BeginTransactionAsync();
            ctx.Database.AutoSavepointsEnabled = false;

            dynamic data = GetData(command);
            dynamic handler = GetHandler(scope, command);

            try
            {
                await handler.Handle(command.Id, data);
            }
            catch (Exception ex)
            {
                ctx.ChangeTracker.Clear();
                ctx.Attach(command);
                command.Error = ex.Message + ex.InnerException?.Message;
            }

            sw.Stop();

            command.Processed(sw.Elapsed.TotalMilliseconds);

            await ctx.SaveChangesAsync();
            ctx.ChangeTracker.Clear();
            await ctx.Database.CommitTransactionAsync();
        }

        await Process(scope, ctx, processorId);
    }

    private static readonly ConcurrentDictionary<string, Type> _types = new();
    private static dynamic GetData(Command command)
    {
        var type = _types.GetOrAdd(command.Type, typeof(Command).Assembly.GetType(command.Type)!);
        dynamic data = JsonConvert.DeserializeObject(command.Data, type)!;
        return data;
    }

    private static readonly ConcurrentDictionary<string, Type> _handlers = new();
    private static dynamic GetHandler(IServiceScope scope, Command command)
    {
        var handlerType = _handlers.GetOrAdd(command.Type, typeof(ICommand).Assembly.GetType($"{command.Type}Handler")!);
        dynamic handler = scope.ServiceProvider.GetRequiredService(handlerType);
        return handler;
    }

    private static readonly string Sql = @"
        UPDATE syki.commands
        SET processor_id = {0}, status = 'Processing'
        WHERE id IN (
            SELECT id
            FROM syki.commands
            WHERE processor_id IS NULL AND (not_before IS NULL OR not_before < NOW())
            ORDER BY created_at
            FOR UPDATE SKIP LOCKED
            LIMIT 10
        )
        RETURNING *;
    ";
}
