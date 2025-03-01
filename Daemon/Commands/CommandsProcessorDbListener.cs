using Dapper;
using Npgsql;
using Hangfire;

namespace Syki.Daemon.Commands;

public class CommandsProcessorDbListener(IConfiguration configuration) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await using var connection = new NpgsqlConnection(configuration.Database().ConnectionString);
        
        await connection.OpenAsync(stoppingToken);

        await CreateTrigger(connection);

        connection.Notification += (o, e) =>
        {
            BackgroundJob.Enqueue<CommandsProcessor>(x => x.Run());
        };

        await using (var cmd = new NpgsqlCommand("LISTEN new_task;", connection))
        {
            await cmd.ExecuteNonQueryAsync(stoppingToken);
        }

        while (!stoppingToken.IsCancellationRequested)
        {
            await connection.WaitAsync(stoppingToken).ConfigureAwait(ConfigureAwaitOptions.SuppressThrowing);
        }
    }

    private async Task CreateTrigger(NpgsqlConnection connection)
    {
        const string sql = @"
            CREATE OR REPLACE FUNCTION notify_new_task_trigger()
            RETURNS trigger
            LANGUAGE 'plpgsql'
            AS $BODY$ 
            BEGIN
                PERFORM pg_notify('new_task', '');
                RETURN NEW;
            END
            $BODY$;

            CREATE OR REPLACE TRIGGER notify_new_task_trigger
            AFTER INSERT ON syki.commands
            FOR EACH ROW EXECUTE PROCEDURE notify_new_task_trigger();
        ";

        await connection.ExecuteAsync(sql);
    }
}
