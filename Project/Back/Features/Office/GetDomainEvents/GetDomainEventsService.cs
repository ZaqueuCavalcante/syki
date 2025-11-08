using Dapper;
using Npgsql;
using Exato.Shared.Features.Office.GetDomainEvents;

namespace Exato.Back.Features.Office.GetDomainEvents;

public class GetDomainEventsService(NpgsqlDataSource dataSource) : IOfficeService
{
    public async Task<GetDomainEventsOut> Get(GetDomainEventsIn data)
    {
        await using var connection = await dataSource.OpenConnectionAsync();

        const string totalSql = @"
            SELECT
                count(DISTINCT e.id)
            FROM
                exato.domain_events e
            LEFT JOIN
                exato.commands c ON c.event_id = e.id
            WHERE
                (@Type IS NULL OR e.type = @Type)
                    AND
                (@Status IS NULL OR e.status = @Status)
                    AND
                (@CommandStatus IS NULL OR c.status = @CommandStatus)
                    AND
                (@OrganizationId IS NULL OR e.organization_id = @OrganizationId)
        ";

        const string typesSql = @"SELECT type FROM exato.domain_events GROUP BY type";

        const string itemsSql = @"
            SELECT
                e.id,
                e.type,
                e.status,
                e.occurred_at,
                e.processed_at,
                COALESCE(
                    ARRAY_AGG(c.status ORDER BY c.created_at) FILTER (WHERE c.status IS NOT NULL),
                    '{}'::text[]
                ) AS commands
            FROM
                exato.domain_events e
            LEFT JOIN
                exato.commands c ON c.event_id = e.id
            WHERE
                (@Type IS NULL OR e.type = @Type)
                    AND
                (@Status IS NULL OR e.status = @Status)
                    AND
                (@CommandStatus IS NULL OR c.status = @CommandStatus)
                    AND
                (@OrganizationId IS NULL OR e.organization_id = @OrganizationId)
            GROUP BY
                e.id
            ORDER BY
                e.occurred_at DESC
            LIMIT 10
            OFFSET @Offset
        ";

        var parameters = new
        {
            data.Type,
            data.OrganizationId,
            Offset = data.Page * 10,
            Status = data.Status?.ToString(),
            CommandStatus = data.CommandStatus?.ToString(),
        };

        var total = await connection.QueryFirstAsync<int>(totalSql, parameters);
        var types = (await connection.QueryAsync<string>(typesSql, parameters)).ToList();
        var events = (await connection.QueryAsync<GetDomainEventsItemOut>(itemsSql, parameters)).ToList();

        if (data.CommandStatus != null)
        {
            events = events.Where(x => x.Commands.Contains(data.CommandStatus.Value)).ToList();
        }

        return new()
        {
            Total = total,
            Types = types,
            Items = events,
        };
    }
}
