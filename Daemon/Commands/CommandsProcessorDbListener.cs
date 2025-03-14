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

        await CreateCommandTrigger(connection);
        await CreateCommandBatchTrigger(connection);

        connection.Notification += (o, e) =>
        {
            var processingJobs = JobStorage.Current.GetMonitoringApi().ProcessingJobs(0, int.MaxValue).Count(x => x.Value.Job.Type == typeof(CommandsProcessor));
            var enqueuedJobs = JobStorage.Current.GetMonitoringApi().EnqueuedJobs("default", 0, int.MaxValue).Count(x => x.Value.Job.Type == typeof(CommandsProcessor));
            if (processingJobs < 15 && enqueuedJobs < 5)
            {
                BackgroundJob.Enqueue<CommandsProcessor>(x => x.Run());
            }
        };

        await using (var cmd = new NpgsqlCommand("LISTEN new_command;", connection))
        {
            await cmd.ExecuteNonQueryAsync(stoppingToken);
        }

        while (!stoppingToken.IsCancellationRequested)
        {
            await connection.WaitAsync(stoppingToken).ConfigureAwait(ConfigureAwaitOptions.SuppressThrowing);
        }
    }

    private static async Task CreateCommandTrigger(NpgsqlConnection connection)
    {
        const string sql = @"
            CREATE OR REPLACE FUNCTION notify_new_command_function()
            RETURNS trigger
            LANGUAGE 'plpgsql'
            AS $BODY$ 
            BEGIN
                PERFORM pg_notify('new_command', '');
                RETURN NEW;
            END
            $BODY$;

            CREATE OR REPLACE TRIGGER notify_new_command_trigger
            AFTER INSERT ON syki.commands
            EXECUTE PROCEDURE notify_new_command_function();
        ";

        await connection.ExecuteAsync(sql);
    }

    private static async Task CreateCommandBatchTrigger(NpgsqlConnection connection)
    {
        const string sql = @"
            CREATE OR REPLACE FUNCTION update_command_batch_function()
            RETURNS trigger
            LANGUAGE 'plpgsql'
            AS $BODY$
            DECLARE
                batch_status text;
                batch_success boolean;
                batch_next_command_id uuid;
            BEGIN
                IF NEW.batch_id IS NULL THEN
                    RETURN NEW;
                END IF;

                SELECT
                    status, next_command_id INTO batch_status, batch_next_command_id
                FROM syki.command_batches
                WHERE id = NEW.batch_id
                FOR UPDATE;

                IF batch_status = 'Error' THEN
                    RETURN NEW;
                END IF;

                IF batch_status = 'Pending' THEN
                    UPDATE syki.command_batches
                    SET status = 'Processing'
                    WHERE id = NEW.batch_id;
                END IF;

                IF NEW.status = 'Error' THEN
                    UPDATE syki.command_batches
                    SET status = 'Error'
                    WHERE id = NEW.batch_id;
                    RETURN NEW;
                END IF;

                SELECT count(1) = 0 INTO batch_success
                FROM syki.commands
                WHERE batch_id = NEW.batch_id AND status <> 'Success';

                IF NOT batch_success THEN
                    RETURN NEW;
                END IF;

                UPDATE syki.command_batches
                SET status = 'Success', processed_at = now()
                WHERE id = NEW.batch_id;

                IF batch_next_command_id IS NOT NULL THEN
                    UPDATE syki.commands
                    SET status = 'Pending'
                    WHERE id = batch_next_command_id;

                    PERFORM pg_notify('new_command', '');
                END IF;

                RETURN NEW;
            END
            $BODY$;

            CREATE OR REPLACE TRIGGER update_command_batch_trigger
            AFTER UPDATE ON syki.commands
            FOR EACH ROW EXECUTE PROCEDURE update_command_batch_function();
        ";

        await connection.ExecuteAsync(sql);
    }
}
