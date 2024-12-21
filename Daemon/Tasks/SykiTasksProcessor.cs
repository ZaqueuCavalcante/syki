using Dapper;
using Npgsql;
using Newtonsoft.Json;
using System.Diagnostics;

namespace Syki.Daemon.Tasks;

public class SykiTasksProcessor(IConfiguration configuration, IServiceScopeFactory serviceScopeFactory)
{
    public async Task Run()
    {
        using var scope = serviceScopeFactory.CreateScope();
        await using var connection = new NpgsqlConnection(configuration.Database().ConnectionString);

        // TODO: Se tiver 1.000.000 de tarefas pendentes e isso aqui rodar, deveria trazer todos de uma vez pra memoria?
        // Analisar como ficar buscando aos poucos, lotes de 100...

        const string sql = @"
            UPDATE syki.tasks
            SET processor_id = @ProcessorId, status = 'Processing'
            WHERE processor_id IS NULL;

            SELECT * FROM syki.tasks
            WHERE processor_id = @ProcessorId AND processed_at IS NULL
            ORDER BY created_at;
        ";

        var tasks = await connection.QueryAsync<SykiTask>(sql, new { ProcessorId = Guid.NewGuid() });

        var sw = Stopwatch.StartNew();

        foreach (var task in tasks)
        {
            sw.Restart();

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
                SET processed_at = now(), status = @Status, error = @Error, duration = @Duration
                WHERE id = @Id
            ";

            var parameters = new
            {
                task.Id,
                error,
                Duration = sw.Elapsed.TotalMilliseconds,
                Status = error.HasValue() ? SykiTaskStatus.Error.ToString() : SykiTaskStatus.Success.ToString(),
            };
            sw.Stop();

            await connection.ExecuteAsync(update, parameters);
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
        var handlerType = typeof(ISykiTask).Assembly.GetType($"{task.Type}Handler")!;
        dynamic handler = scope.ServiceProvider.GetRequiredService(handlerType);
        return handler;
    }
}
