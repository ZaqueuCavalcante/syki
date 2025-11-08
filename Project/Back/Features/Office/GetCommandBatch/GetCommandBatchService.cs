using Dapper;
using Npgsql;
using Exato.Shared.Features.Office.GetCommandBatch;

namespace Exato.Back.Features.Office.GetCommandBatch;

public class GetCommandBatchService(NpgsqlDataSource dataSource) : IOfficeService
{
    public async Task<OneOf<GetCommandBatchOut, ExatoError>> Get(Guid id)
    {
        await using var connection = await dataSource.OpenConnectionAsync();

        const string batchSql = @"
            SELECT *
            FROM exato.command_batches
            WHERE id = @Id
        ";

        var parameters = new { id };

        var command = await connection.QueryFirstOrDefaultAsync<GetCommandBatchOut>(batchSql, parameters);

        if (command == null) return CommandBatchNotFound.I;
   
        return command;
    }
}
