using Dapper;
using Npgsql;

namespace Syki.Back.Features.Adm.GetBatch;

public class GetBatchService(DatabaseSettings settings) : IAdmService
{
    public async Task<BatchOut> Get(Guid id)
    {
        await using var dataSource = NpgsqlDataSource.Create(settings.ConnectionString);
        await using var connection = await dataSource.OpenConnectionAsync();

        const string sql = @"
            SELECT
                type,
                size,
                status,
                created_at,
                processed_at,
                event_id,
                source_command_id,
                next_command_id
            FROM
                syki.command_batches
            WHERE
                id = @Id
        ";

        var parameters = new { id };
        var batch = await connection.QueryFirstOrDefaultAsync<BatchOut>(sql, parameters);

        const string commandsSql = @"
            SELECT *
            FROM syki.commands
            WHERE batch_id = @Id
        ";
        batch.Commands = (await connection.QueryAsync<CommandOut>(commandsSql, parameters)).ToList();
        batch.Commands.ForEach(x => x.Description = x.Type.ToCommandDescription());

        return batch;
    }
}
