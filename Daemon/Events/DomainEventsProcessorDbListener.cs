using Dapper;
using Npgsql;
using Hangfire;

namespace Syki.Daemon.Events;

public class DomainEventsProcessorDbListener(IConfiguration configuration) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await using var connection = new NpgsqlConnection(configuration.Database().ConnectionString);
        await connection.OpenAsync(stoppingToken);

        await CreateTrigger(connection);

        connection.Notification += (o, e) =>
        {
            var processingJobs = JobStorage.Current.GetMonitoringApi().ProcessingJobs(0, int.MaxValue).Count(x => x.Value.Job.Type == typeof(DomainEventsProcessor));
            var enqueuedJobs = JobStorage.Current.GetMonitoringApi().EnqueuedJobs("default", 0, int.MaxValue).Count(x => x.Value.Job.Type == typeof(DomainEventsProcessor));
            if (processingJobs < 3 && enqueuedJobs < 5)
            {
                BackgroundJob.Enqueue<DomainEventsProcessor>(x => x.Run());
            }
        };

        await using (var cmd = new NpgsqlCommand("LISTEN new_domain_event;", connection))
        {
            await cmd.ExecuteNonQueryAsync(stoppingToken);
        }

        while (!stoppingToken.IsCancellationRequested)
        {
            await connection.WaitAsync(stoppingToken).ConfigureAwait(ConfigureAwaitOptions.SuppressThrowing);
        }
    }

    private static async Task CreateTrigger(NpgsqlConnection connection)
    {
        const string sql = @"
            CREATE OR REPLACE FUNCTION notify_new_domain_event_function()
            RETURNS trigger
            LANGUAGE 'plpgsql'
            AS $BODY$ 
            BEGIN
                PERFORM pg_notify('new_domain_event', '');
                RETURN NEW;
            END
            $BODY$;

            CREATE OR REPLACE TRIGGER notify_new_domain_event_trigger
            AFTER INSERT ON syki.domain_events
            EXECUTE PROCEDURE notify_new_domain_event_function();
        ";

        await connection.ExecuteAsync(sql);
    }
}
