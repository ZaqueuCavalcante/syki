using Dapper;
using Npgsql;

namespace Syki.Back.Features.Adm.GetDomainEvents;

public class GetDomainEventsService(DatabaseSettings settings) : IAdmService
{
    public async Task<List<DomainEventTableOut>> Get(DomainEventTableFilterIn filters)
    {
        await using var dataSource = NpgsqlDataSource.Create(settings.ConnectionString);
        await using var connection = await dataSource.OpenConnectionAsync();

        const string sql = @"
            SELECT
                e.id,
                e.type,
                e.status,
                e.occurred_at,
                e.processed_at,
                ARRAY_AGG(t.status ORDER BY t.created_at) AS commands
            FROM
                syki.domain_events e
            LEFT JOIN
                syki.commands t ON t.event_id = e.id
            WHERE
                (@Type IS NULL OR e.type = @Type)
                    AND
                (@Status IS NULL OR e.status = @Status)
                    AND
                (@InstitutionId IS NULL OR e.institution_id = @InstitutionId)
            GROUP BY
                e.id
            ORDER BY
                e.occurred_at DESC
        ";

        var parameters = new
        {
            filters.Type,
            filters.InstitutionId,
            Status = filters.Status?.ToString(),
        };

        var events = (await connection.QueryAsync<DomainEventTableOut>(sql, parameters)).ToList();

        if (filters.Commands != null)
        {
            events = events.Where(x => x.Commands.Contains(filters.Commands.Value)).ToList();
        }

        events.ForEach(x => x.Description = x.Type.ToDomainEventDescription());

        return events;
    }
}
