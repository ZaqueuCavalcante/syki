using Dapper;
using Npgsql;
using Newtonsoft.Json;
using Syki.Back.Settings;

namespace Syki.Back.Tasks;

public class SykiTasksProcessor : BackgroundService
{
    private readonly DatabaseSettings _dbSettings;
    private readonly  IServiceScopeFactory _serviceScopeFactory;
    public SykiTasksProcessor(
        DatabaseSettings dbSettings,
        IServiceScopeFactory serviceScopeFactory
    ) {
        _dbSettings = dbSettings;
        _serviceScopeFactory = serviceScopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var scope = _serviceScopeFactory.CreateScope();
        using var timer = new PeriodicTimer(TimeSpan.FromSeconds(1000_000));
        using var connection = new NpgsqlConnection(_dbSettings.ConnectionString);

        while (!stoppingToken.IsCancellationRequested && await timer.WaitForNextTickAsync(stoppingToken))
        {
            var task = await connection.QueryFirstOrDefaultAsync<SykiTask>(sql);
            if (task == null)
                continue;

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

    private static dynamic GetData(SykiTask task)
    {
        var type = typeof(SykiTask).Assembly.GetType(task.Type)!;
        dynamic data = JsonConvert.DeserializeObject(task.Data.ToString()!, type)!;
        return data;
    }

    private static dynamic GetHanlder(IServiceScope scope, SykiTask task)
    {
        var handlerType = typeof(SykiTask).Assembly.GetType($"{task.Type}Handler")!;
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
            1
        FOR UPDATE
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
