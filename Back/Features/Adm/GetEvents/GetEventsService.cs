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

        const string eventsSql = @"
            SELECT type, status, created_at, processed_at, duration
            FROM syki.domain_events
            ORDER BY created_at DESC
        ";

        result.Summary = await connection.QueryFirstAsync<EventsSummaryOut>(summarySql);
        result.LastEvents = (await connection.QueryAsync<LastEventOut>(lastEventsSql)).ToList();
        result.EventTypes = (await connection.QueryAsync<EventTypeCountOut>(typesSql)).ToList();
        result.Events = (await connection.QueryAsync<EventTableOut>(eventsSql)).ToList();

        result.EventTypes.ForEach(x => x.Type = x.Type.ToPortugueseEventName());
        result.Events.ForEach(x => x.Type = x.Type.ToPortugueseEventName());

        return result;
    }
}
