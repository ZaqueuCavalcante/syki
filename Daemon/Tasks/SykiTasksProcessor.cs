using Dapper;
using Npgsql;
using Newtonsoft.Json;
using Syki.Back.Tasks;
using Syki.Back.Settings;
using Syki.Daemon.Settings;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Syki.Daemon.Tasks;

public class SykiTasksProcessor : BackgroundService
{
    private readonly TasksSettings _settings;
    private readonly DatabaseSettings _dbSettings;
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly IConfiguration _config;
    public SykiTasksProcessor(
        TasksSettings settings,
        DatabaseSettings dbSettings,
        IServiceScopeFactory serviceScopeFactory,
        IConfiguration config
    ) {
        _config = config;
        _settings = settings;
        _dbSettings = dbSettings;
        _serviceScopeFactory = serviceScopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var scope = _serviceScopeFactory.CreateScope();
        using var timer = new PeriodicTimer(TimeSpan.FromSeconds(_settings.Delay));
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
