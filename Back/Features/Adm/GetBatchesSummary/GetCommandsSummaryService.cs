using Dapper;
using Npgsql;

namespace Syki.Back.Features.Adm.GetBatchesSummary;

public class GetBatchesSummaryService(NpgsqlDataSource dataSource) : IAdmService
{
    public async Task<GetBatchesSummaryOut> Get()
    {
        await using var connection = await dataSource.OpenConnectionAsync();

        var result = new GetBatchesSummaryOut();

        const string summarySql = @"
            SELECT
                count(1) AS total,
                count(1) FILTER (WHERE status = 'Pending') AS pending,
                count(1) FILTER (WHERE status = 'Processing') AS processing,
                count(1) FILTER (WHERE status = 'Success') AS success,
                count(1) FILTER (WHERE status = 'Error') AS error
            FROM syki.command_batches
        ";

        const string typesSql = @"
            SELECT type, count(1) AS total
            FROM syki.command_batches
            GROUP BY type
            ORDER BY total DESC
        ";

        const string institutionsSql = @"
            SELECT id, name
            FROM syki.institutions
            WHERE id <> '00000000-0000-0000-0000-000000000000'
            ORDER BY name
        ";

        result.Summary = await connection.QueryFirstAsync<BatchesSummaryOut>(summarySql);
        result.Types = (await connection.QueryAsync<BatchTypeCountOut>(typesSql)).ToList();
        result.Institutions = (await connection.QueryAsync<TinyInstitutionOut>(institutionsSql)).ToList();

        return result;
    }
}
