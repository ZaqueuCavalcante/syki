using Dapper;
using Npgsql;

namespace Syki.Back.Features.Adm.GetCommandsSummary;

public class GetCommandsSummaryService(NpgsqlDataSource dataSource) : IAdmService
{
    public async Task<GetCommandsSummaryOut> Get()
    {
        await using var connection = await dataSource.OpenConnectionAsync();

        var result = new GetCommandsSummaryOut();

        const string summarySql = @"
            SELECT
                count(1) AS total,
                count(1) FILTER (WHERE status = 'Pending') AS pending,
                count(1) FILTER (WHERE status = 'Processing') AS processing,
                count(1) FILTER (WHERE status = 'Success') AS success,
                count(1) FILTER (WHERE status = 'Error') AS error
            FROM syki.commands
        ";

        const string typesSql = @"
            SELECT type, count(1) AS total
            FROM syki.commands
            GROUP BY TYPE
            ORDER BY total DESC
        ";

        const string institutionsSql = @"
            SELECT id, name
            FROM syki.institutions
            WHERE id <> '00000000-0000-0000-0000-000000000000'
            ORDER BY name
        ";

        result.Summary = await connection.QueryFirstAsync<CommandsSummaryOut>(summarySql);
        result.Types = (await connection.QueryAsync<CommandTypeCountOut>(typesSql)).ToList();
        result.Institutions = (await connection.QueryAsync<TinyInstitutionOut>(institutionsSql)).ToList();

        result.Types.ForEach(x => x.Description = x.Type.ToCommandDescription());

        return result;
    }
}
