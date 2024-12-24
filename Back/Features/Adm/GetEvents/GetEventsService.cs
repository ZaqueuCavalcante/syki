using Dapper;
using Npgsql;

namespace Syki.Back.Features.Adm.GetEvents;

public class GetEventsService(DatabaseSettings settings) : IAdmService
{
    public async Task<GetEventsOut> Get()
    {
        using var connection = new NpgsqlConnection(settings.ConnectionString);

        var result = new GetEventsOut();

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
            SELECT created_at::timestamp(0) AS date, count(1) AS total
            FROM syki.domain_events
            GROUP BY created_at::timestamp(0)
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

        result.Summary = await connection.QueryFirstAsync<EventsSummaryOut>(summarySql);
        result.LastEvents = (await connection.QueryAsync<LastEventOut>(lastEventsSql)).ToList();
        result.EventTypes = (await connection.QueryAsync<EventTypeCountOut>(typesSql)).ToList();
        result.Institutions = (await connection.QueryAsync<TinyInstitutionOut>(institutionsSql)).ToList();

        result.EventTypes.ForEach(x => x.Description = x.Type.ToDomainEventDescription());

        return result;
    }
}
