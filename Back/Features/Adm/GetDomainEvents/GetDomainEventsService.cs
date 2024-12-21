using Dapper;
using Npgsql;

namespace Syki.Back.Features.Adm.GetDomainEvents;

public class GetDomainEventsService(DatabaseSettings settings) : IAdmService
{
    public async Task<List<DomainEventTableOut>> Get(DomainEventTableFilterIn filters)
    {
        using var connection = new NpgsqlConnection(settings.ConnectionString);

        const string sql = @"
            SELECT
                type,
                status,
                created_at,
                processed_at,
                duration
            FROM
                syki.domain_events
            WHERE
                (@Type IS NULL OR type = @Type)
                    AND
                (@Status IS NULL OR status = @Status)
            ORDER BY
                created_at DESC
        ";

        var parameters = new
        {
            filters.Type,
            Status = filters.Status?.ToString()
        };

        var events = (await connection.QueryAsync<DomainEventTableOut>(sql, parameters)).ToList();

        events.ForEach(x => x.Type = x.Type.ToPortugueseEventName());

        return events;
    }
}
