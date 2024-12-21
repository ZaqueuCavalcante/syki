using Dapper;
using Npgsql;

namespace Syki.Back.Features.Adm.GetDomainEvent;

public class GetDomainEventService(DatabaseSettings settings) : IAdmService
{
    public async Task<DomainEventOut> Get(Guid id)
    {
        using var connection = new NpgsqlConnection(settings.ConnectionString);

        const string sql = @"
            SELECT *
            FROM syki.domain_events
            WHERE id = @Id
        ";
        var evt = await connection.QueryFirstOrDefaultAsync<DomainEventOut>(sql, new{ id }) ?? new();
        evt.Type = evt.Type.ToDomainEventDescription();

        const string tasksSql = @"
            SELECT *
            FROM syki.tasks
            WHERE event_id = @Id
        ";
        evt.Tasks = (await connection.QueryAsync<DomainEventTaskOut>(tasksSql, new{ id })).ToList();

        return evt;
    }
}
