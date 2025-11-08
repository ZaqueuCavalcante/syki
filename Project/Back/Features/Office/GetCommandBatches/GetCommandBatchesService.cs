using Dapper;
using Npgsql;
using Exato.Shared.Features.Office.GetCommandBatches;

namespace Exato.Back.Features.Office.GetCommandBatches;

public class GetCommandBatchesService(NpgsqlDataSource dataSource) : IOfficeService
{
    public async Task<GetCommandBatchesOut> Get(GetCommandBatchesIn data)
    {
        await using var connection = await dataSource.OpenConnectionAsync();

        const string totalSql = @"
            SELECT
                count(1)
            FROM
                exato.command_batches
            WHERE
                (@Type IS NULL OR type = @Type)
                    AND
                (@Status IS NULL OR status = @Status)
        ";

        const string itemsSql = @"
            SELECT
                id,
                type,
                size,
                status,
                created_at,
                processed_at
            FROM
                exato.command_batches
            WHERE
                (@Type IS NULL OR type = @Type)
                    AND
                (@Status IS NULL OR status = @Status)
            ORDER BY
                created_at DESC
            LIMIT 10
            OFFSET @Offset
        ";

        var parameters = new
        {
            Offset = data.Page * 10,
            Type = data.Type?.ToString(),
            Status = data.Status?.ToString(),
        };

        var total = await connection.QueryFirstAsync<int>(totalSql, parameters);
        var batches = (await connection.QueryAsync<GetCommandBatchesItemOut>(itemsSql, parameters)).ToList();

        return new()
        {
            Total = total,
            Items = batches,
        };
    }
}
