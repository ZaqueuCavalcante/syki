using Dapper;
using Npgsql;
using Newtonsoft.Json;
using System.Diagnostics;

namespace Syki.Daemon.Commands;

public class CommandsProcessor(IConfiguration configuration, IServiceScopeFactory serviceScopeFactory)
{
    public async Task Run()
    {
        using var scope = serviceScopeFactory.CreateScope();
        await using var connection = new NpgsqlConnection(configuration.Database().ConnectionString);

        const string sql = @"
            UPDATE syki.commands
            SET processor_id = @ProcessorId, status = 'Processing'
            WHERE processor_id IS NULL;

            SELECT id, type, data
            FROM syki.commands
            WHERE processor_id = @ProcessorId AND processed_at IS NULL
            ORDER BY created_at;
        ";

        var commands = await connection.QueryAsync<Command>(sql, new { ProcessorId = Guid.NewGuid() });

        var sw = Stopwatch.StartNew();

        foreach (var command in commands)
        {
            sw.Restart();

            dynamic data = GetData(command);
            dynamic handler = GetHandler(scope, command);
            string? error = null;

            try
            {
                await handler.Handle(data);
            }
            catch (Exception ex)
            {
                error = ex.Message + ex.InnerException?.Message;
            }

            const string update = @"
                UPDATE syki.commands
                SET processed_at = now(), status = @Status, error = @Error, duration = @Duration
                WHERE id = @Id
            ";

            var parameters = new
            {
                command.Id,
                error,
                Duration = sw.Elapsed.TotalMilliseconds,
                Status = error.HasValue() ? CommandStatus.Error.ToString() : CommandStatus.Success.ToString(),
            };
            sw.Stop();

            await connection.ExecuteAsync(update, parameters);
        }
    }

    private static dynamic GetData(Command command)
    {
        var type = typeof(Command).Assembly.GetType(command.Type)!;
        dynamic data = JsonConvert.DeserializeObject(command.Data, type)!;
        return data;
    }

    private static dynamic GetHandler(IServiceScope scope, Command command)
    {
        var handlerType = typeof(ICommand).Assembly.GetType($"{command.Type}Handler")!;
        dynamic handler = scope.ServiceProvider.GetRequiredService(handlerType);
        return handler;
    }
}
