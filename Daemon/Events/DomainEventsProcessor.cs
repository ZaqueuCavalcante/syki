using Dapper;
using Npgsql;
using Newtonsoft.Json;
using Syki.Back.Events;

namespace Syki.Daemon.Events;

public class DomainEventsProcessor(IConfiguration configuration, IServiceScopeFactory serviceScopeFactory)
{
    public async Task Run()
    {
        using var scope = serviceScopeFactory.CreateScope();
        await using var connection = new NpgsqlConnection(configuration.Database().ConnectionString);

        // TODO: CALL BOTH QUERIES IN SAME REQUEST

        const string pickRowsSql = @"
            UPDATE syki.domain_events
            SET processor_id = @ProcessorId
            WHERE processor_id IS NULL
        ";

        var processorId = Guid.NewGuid();
        var rows = await connection.ExecuteAsync(pickRowsSql, new { processorId });
        if (rows == 0) return;

        const string sql = @"
            SELECT * FROM syki.domain_events
            WHERE processor_id = @ProcessorId AND processed_at IS NULL
        ";

        var events = await connection.QueryAsync<DomainEvent>(sql, new { processorId });

        foreach (var evt in events)
        {
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
                SET processed_at = now(), error = @Error
                WHERE id = @Id
            ";

            await connection.ExecuteAsync(update, new { evt.Id, error });
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