using Dapper;
using Npgsql;
using Exato.Shared.Features.Office.GetCommands;

namespace Exato.Back.Features.Office.GetCommands;

public class GetCommandsService(NpgsqlDataSource dataSource) : IOfficeService
{
    public async Task<GetCommandsOut> Get(GetCommandsIn data)
    {
        await using var connection = await dataSource.OpenConnectionAsync();

        const string totalSql = @"
            SELECT
                count(1)
            FROM
                exato.commands
            WHERE
                (@Type IS NULL OR type = @Type)
                    AND
                (@Status IS NULL OR status = @Status)
        ";

        const string typesSql = @"SELECT type FROM exato.commands GROUP BY type";

        const string itemsSql = @"
            SELECT
                id,
                type,
                status,
                duration,
                created_at,
                processed_at
            FROM
                exato.commands
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
            data.Type,
            Offset = data.Page * 10,
            Status = data.Status?.ToString(),
        };

        var total = await connection.QueryFirstAsync<int>(totalSql, parameters);
        var types = (await connection.QueryAsync<string>(typesSql, parameters)).ToList();
        var commands = (await connection.QueryAsync<GetCommandsItemOut>(itemsSql, parameters)).ToList();

        return new()
        {
            Total = total,
            Types = types,
            Items = commands,
        };
    }
}
