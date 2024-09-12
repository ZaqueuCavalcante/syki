using Dapper;
using Npgsql;
using Newtonsoft.Json;

namespace Syki.Daemon.Tasks;

public class SykiTasksProcessor(IConfiguration configuration, IServiceScopeFactory serviceScopeFactory)
{
    public async Task Run()
    {
        using var scope = serviceScopeFactory.CreateScope();
        await using var connection = new NpgsqlConnection(configuration.Database().ConnectionString);

        const string pickRowsSql = @"
            UPDATE syki.tasks
            SET processor_id = @ProcessorId
            WHERE processor_id IS NULL
        ";

        var processorId = Guid.NewGuid();
        var rows = await connection.ExecuteAsync(pickRowsSql, new { processorId });
        if (rows == 0) return;

        const string sql = @"
            SELECT * FROM syki.tasks
            WHERE processor_id = @ProcessorId AND processed_at IS NULL
        ";

        var tasks = await connection.QueryAsync<SykiTask>(sql, new { processorId });

        foreach (var task in tasks)
        {
            dynamic data = GetData(task);
            dynamic handler = GetHandler(scope, task);
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
                UPDATE syki.tasks
                SET processed_at = now(), error = @Error
                WHERE id = @Id
            ";

            await connection.ExecuteAsync(update, new { task.Id, error });
        }
    }

    private static dynamic GetData(SykiTask task)
    {
        var type = typeof(SykiTask).Assembly.GetType(task.Type)!;
        dynamic data = JsonConvert.DeserializeObject(task.Data, type)!;
        return data;
    }

    private static dynamic GetHandler(IServiceScope scope, SykiTask task)
    {
        var type = task.Type.Split(".").Last();
        var handlerType = typeof(SykiTasksProcessor).Assembly.GetType($"Syki.Daemon.Tasks.{type}Handler")!;
        dynamic handler = scope.ServiceProvider.GetRequiredService(handlerType);
        return handler;
    }
}
