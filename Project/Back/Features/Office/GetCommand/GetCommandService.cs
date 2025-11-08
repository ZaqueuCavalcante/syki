using Dapper;
using Npgsql;
using Exato.Shared.Features.Office.GetCommand;

namespace Exato.Back.Features.Office.GetCommand;

public class GetCommandService(NpgsqlDataSource dataSource) : IOfficeService
{
    public async Task<OneOf<GetCommandOut, ExatoError>> Get(Guid id)
    {
        await using var connection = await dataSource.OpenConnectionAsync();

        const string sql = @"
            SELECT *
            FROM exato.commands
            WHERE id = @Id
        ";

        var parameters = new { id };

        var command = await connection.QueryFirstOrDefaultAsync<GetCommandOut>(sql, parameters);

        if (command == null) return CommandNotFound.I;

        const string retriesSql = @"
            SELECT *
            FROM exato.commands
            WHERE original_id = @Id
            ORDER BY created_at DESC
        ";
        command.Retries = (await connection.QueryAsync<GetCommandOut>(retriesSql, parameters)).ToList();

        return command;
    }
}
