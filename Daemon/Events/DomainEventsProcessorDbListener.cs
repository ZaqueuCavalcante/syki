using Dapper;
using Npgsql;

namespace Syki.Daemon.Events;

public class DomainEventsProcessorDbListener(IConfiguration configuration, IServiceScopeFactory serviceScopeFactory) : BackgroundService
{
    private readonly SemaphoreSlim _throttler = new(5);

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await using var connection = new NpgsqlConnection(configuration.Database().ConnectionString);
        await connection.OpenAsync(stoppingToken);

        await CreateTrigger(connection);

        _ = Task.Run(async () =>
        {
            var processor = new DomainEventsProcessor(serviceScopeFactory);
            await processor.Run();
        }, CancellationToken.None);

        connection.Notification += async (o, e) =>
        {
            await _throttler.WaitAsync(stoppingToken);

            _ = Task.Run(async () =>
            {
                try
                {
                    var processor = new DomainEventsProcessor(serviceScopeFactory);
                    await processor.Run();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error processing domain event: {ex.Message}");
                }
                finally
                {
                    _throttler.Release();
                }
            }, CancellationToken.None);
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
