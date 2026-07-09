using Dapper;
using Npgsql;

namespace Estud.Back.Features.Adm.GetBatch;

public class GetBatchService(NpgsqlDataSource dataSource) : IEstudService
{
    public async Task<BatchOut> Get(Guid id)
    {
        await using var connection = await dataSource.OpenConnectionAsync();

        const string sql = @"
            SELECT
                type,
                size,
                status,
                created_at,
                processed_at,
                source_command_id,
                next_command_id
            FROM
                estud.command_batches
            WHERE
                id = @Id
        ";

        var parameters = new { id };
        var batch = await connection.QueryFirstOrDefaultAsync<BatchOut>(sql, parameters);

        const string commandsSql = @"
            SELECT *
            FROM estud.commands
            WHERE batch_id = @Id
        ";
        batch.Commands = (await connection.QueryAsync<CommandOut>(commandsSql, parameters)).ToList();
        batch.Commands.ForEach(x => x.Description = x.Type.ToCommandDescription());

        return batch;
    }
}
