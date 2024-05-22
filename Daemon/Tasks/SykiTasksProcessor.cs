using Dapper;
using Npgsql;
using Newtonsoft.Json;
using Syki.Daemon.Settings;

namespace Syki.Daemon.Tasks;

public class SykiTasksProcessor(
    TasksSettings settings,
    DatabaseSettings dbSettings,
    IServiceScopeFactory serviceScopeFactory)
    : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var scope = serviceScopeFactory.CreateScope();
        using var timer = new PeriodicTimer(TimeSpan.FromSeconds(settings.Delay));
        using var connection = new NpgsqlConnection(dbSettings.ConnectionString);

        while (!stoppingToken.IsCancellationRequested && await timer.WaitForNextTickAsync(stoppingToken))
        {
            var tasks = await connection.QueryAsync<SykiTask>(sql);
            if (!tasks.Any())
                continue;

            foreach (var task in tasks)
            {
                dynamic data = GetData(task);
                dynamic handler = GetHanlder(scope, task);
                string? error = null;

                try
                {
                    await handler.Handle(data);
                }
                catch (Exception ex)
                {
                    error = ex.Message + ex.InnerException?.Message;
                }

                await connection.ExecuteAsync(update, new { task.Id, error });
            }
        }
    }

    private static dynamic GetData(SykiTask task)
    {
        var type = typeof(SykiTask).Assembly.GetType(task.Type)!;
        dynamic data = JsonConvert.DeserializeObject(task.Data.ToString()!, type)!;
        return data;
    }

    private static dynamic GetHanlder(IServiceScope scope, SykiTask task)
    {
        var type = task.Type.Split(".").Last();
        var handlerType = typeof(SykiTasksProcessor).Assembly.GetType($"Syki.Daemon.Tasks.{type}Handler")!;
        dynamic handler = scope.ServiceProvider.GetRequiredService(handlerType);
        return handler;
    }

    private const string sql = @"
        SELECT
            *
        FROM
            syki.tasks
        WHERE
            processed_at IS NULL
        ORDER BY
            created_at ASC
        LIMIT
            500
        FOR UPDATE SKIP LOCKED
    ";

    private const string update = @"
        UPDATE
            syki.tasks
        SET
            processed_at = now(),
            error = @Error
        WHERE
            id = @Id
    ";
}
