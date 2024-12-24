using Dapper;
using Npgsql;

namespace Syki.Back.Features.Adm.GetSykiTasksSummary;

public class GetSykiTasksSummaryService(DatabaseSettings settings) : IAdmService
{
    public async Task<GetTasksSummaryOut> Get()
    {
        using var connection = new NpgsqlConnection(settings.ConnectionString);

        var result = new GetTasksSummaryOut();

        const string summarySql = @"
            SELECT
                count(1) AS total,
                count(1) FILTER (WHERE status = 'Pending') AS pending,
                count(1) FILTER (WHERE status = 'Processing') AS processing,
                count(1) FILTER (WHERE status = 'Success') AS success,
                count(1) FILTER (WHERE status = 'Error') AS error
            FROM syki.tasks
        ";

        const string typesSql = @"
            SELECT type, count(1) AS total
            FROM syki.tasks
            GROUP BY TYPE
            ORDER BY total DESC
        ";

        const string institutionsSql = @"
            SELECT id, name
            FROM syki.institutions
            WHERE id <> '00000000-0000-0000-0000-000000000000'
            ORDER BY name
        ";

        result.Summary = await connection.QueryFirstAsync<TasksSummaryOut>(summarySql);
        result.TaskTypes = (await connection.QueryAsync<TaskTypeCountOut>(typesSql)).ToList();
        result.Institutions = (await connection.QueryAsync<TinyInstitutionOut>(institutionsSql)).ToList();

        result.TaskTypes.ForEach(x => x.Description = x.Type.ToSykiTaskDescription());

        return result;
    }
}
