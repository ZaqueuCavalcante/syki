using Npgsql;
using Exato.Back.Settings;

namespace Exato.Workers.Events;

public class NewDomainEventsDbListener(IConfiguration configuration, IServiceScopeFactory serviceScopeFactory) : BackgroundService
{
    private readonly SemaphoreSlim _throttler = new(5);

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await using var connection = new NpgsqlConnection(configuration.Database.Exato);
        await connection.OpenAsync(stoppingToken);

        _ = Task.Run(async () =>
        {
            var processor = new DomainEventsProcessor(serviceScopeFactory);
            await processor.Run();
        }, CancellationToken.None);

        connection.Notification += async (o, e) =>
        {
            if (!await _throttler.WaitAsync(0, CancellationToken.None))
                return;

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
}
