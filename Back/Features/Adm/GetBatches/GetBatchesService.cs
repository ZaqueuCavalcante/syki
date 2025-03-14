using Dapper;
using Npgsql;

namespace Syki.Back.Features.Adm.GetBatches;

public class GetBatchesService(DatabaseSettings settings) : IAdmService
{
    public async Task<List<CommandBatchTableOut>> Get(CommandBatchTableFilterIn filters)
    {
        await using var dataSource = NpgsqlDataSource.Create(settings.ConnectionString);
        await using var connection = await dataSource.OpenConnectionAsync();

        const string sql = @"
            SELECT
                id,
                type,
                size,
                status,
                created_at,
                processed_at
            FROM
                syki.command_batches
            WHERE
                (@Type IS NULL OR type = @Type)
                    AND
                (@Status IS NULL OR status = @Status)
                    AND
                (@InstitutionId IS NULL OR institution_id = @InstitutionId)
            ORDER BY
                created_at DESC
        ";

        var parameters = new
        {
            filters.InstitutionId,
            Type = filters.Type?.ToString(),
            Status = filters.Status?.ToString(),
        };

        return (await connection.QueryAsync<CommandBatchTableOut>(sql, parameters)).ToList();
    }
}
