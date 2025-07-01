using Dapper;
using Npgsql;

namespace Syki.Back.Features.Adm.GetDomainEventsSummary;

public class GetDomainEventsSummaryService(NpgsqlDataSource dataSource) : IAdmService
{
    public async Task<GetDomainEventsSummaryOut> Get()
    {
        await using var connection = await dataSource.OpenConnectionAsync();

        var result = new GetDomainEventsSummaryOut();

        const string summarySql = @"
            SELECT
                count(1) AS total,
                count(1) FILTER (WHERE status = 'Pending') AS pending,
                count(1) FILTER (WHERE status = 'Processing') AS processing,
                count(1) FILTER (WHERE status = 'Success') AS success,
                count(1) FILTER (WHERE status = 'Error') AS error
            FROM syki.domain_events
        ";

        const string lastEventsSql = @"
            SELECT occurred_at::timestamp(0) AS date, count(1) AS total
            FROM syki.domain_events
            GROUP BY occurred_at::timestamp(0)
            ORDER BY date
        ";

        const string typesSql = @"
            SELECT type, count(1) AS total
            FROM syki.domain_events
            GROUP BY TYPE
            ORDER BY total DESC
        ";

        const string institutionsSql = @"
            SELECT id, name
            FROM syki.institutions
            WHERE id <> '00000000-0000-0000-0000-000000000000'
            ORDER BY name
        ";

        result.Summary = await connection.QueryFirstAsync<DomainEventsSummaryOut>(summarySql);
        result.LastEvents = (await connection.QueryAsync<LastDomainEventOut>(lastEventsSql)).ToList();
        result.EventTypes = (await connection.QueryAsync<DomainEventTypeCountOut>(typesSql)).ToList();
        result.Institutions = (await connection.QueryAsync<TinyInstitutionOut>(institutionsSql)).ToList();

        result.EventTypes.ForEach(x => x.Description = x.Type.ToDomainEventDescription());

        return result;
    }
}
