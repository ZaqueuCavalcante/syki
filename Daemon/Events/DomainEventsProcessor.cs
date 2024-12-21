using Dapper;
using Npgsql;
using Newtonsoft.Json;
using Syki.Back.Events;
using System.Diagnostics;

namespace Syki.Daemon.Events;

public class DomainEventsProcessor(IConfiguration configuration, IServiceScopeFactory serviceScopeFactory)
{
    public async Task Run()
    {
        using var scope = serviceScopeFactory.CreateScope();
        await using var connection = new NpgsqlConnection(configuration.Database().ConnectionString);

        // TODO: Se tiver 1.000.000 de eventos pendentes e isso aqui rodar, deveria trazer todos de uma vez pra memoria?
        // Analisar como ficar buscando aos poucos, lotes de 100...

        const string sql = @"
            UPDATE syki.domain_events
            SET processor_id = @ProcessorId, status = 'Processing'
            WHERE processor_id IS NULL;

            SELECT * FROM syki.domain_events
            WHERE processor_id = @ProcessorId AND processed_at IS NULL
            ORDER BY created_at;
        ";

        var events = await connection.QueryAsync<DomainEvent>(sql, new { ProcessorId = Guid.NewGuid() });

        var sw = Stopwatch.StartNew();

        foreach (var evt in events)
        {
            sw.Restart();

            dynamic data = GetData(evt);
            dynamic handler = GetHandler(scope, evt);
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
                UPDATE syki.domain_events
                SET processed_at = now(), status = @Status, error = @Error, duration = @Duration
                WHERE id = @Id
            ";

            var parameters = new
            {
                evt.Id,
                error,
                Duration = sw.Elapsed.TotalMilliseconds,
                Status = error.HasValue() ? DomainEventStatus.Error.ToString() : DomainEventStatus.Success.ToString(),
            };
            sw.Stop();
            await connection.ExecuteAsync(update, parameters);
        }
    }

    private static dynamic GetData(DomainEvent evt)
    {
        var type = typeof(DomainEvent).Assembly.GetType(evt.Type)!;
        dynamic data = JsonConvert.DeserializeObject(evt.Data, type)!;
        return data;
    }

    private static dynamic GetHandler(IServiceScope scope, DomainEvent evt)
    {
        var handlerType = typeof(IDomainEvent).Assembly.GetType($"{evt.Type}Handler")!;
        dynamic handler = scope.ServiceProvider.GetRequiredService(handlerType);
        return handler;
    }
}
