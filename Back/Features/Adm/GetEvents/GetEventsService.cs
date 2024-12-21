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
                count(1) FILTER (WHERE processed_at IS NOT NULL) AS processed,
                count(1) FILTER (WHERE processed_at IS NULL) AS pending,
                count(1) FILTER (WHERE error IS NOT NULL) AS error
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

        result.Summary = await connection.QueryFirstAsync<EventsSummaryOut>(summarySql);
        result.LastEvents = (await connection.QueryAsync<LastEventOut>(lastEventsSql)).ToList();
        result.EventTypes = (await connection.QueryAsync<EventTypeCountOut>(typesSql)).ToList();

        result.EventTypes.ForEach(x => x.Type = x.Type.ToPortugueseEventName());

        return result;
    }
}
