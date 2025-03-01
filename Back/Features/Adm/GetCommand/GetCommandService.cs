using Dapper;
using Npgsql;

namespace Syki.Back.Features.Adm.GetCommand;

public class GetCommandService(DatabaseSettings settings) : IAdmService
{
    public async Task<CommandOut> Get(Guid id)
    {
        await using var dataSource = NpgsqlDataSource.Create(settings.ConnectionString);
        await using var connection = await dataSource.OpenConnectionAsync();

        const string sql = @"
            SELECT *
            FROM syki.commands
            WHERE id = @Id
        ";
        var command = await connection.QueryFirstOrDefaultAsync<CommandOut>(sql, new{ id }) ?? new();
        command.Description = command.Type.ToCommandDescription();

        const string commandsSql = @"
            SELECT *
            FROM syki.commands
            WHERE parent_id = @Id
        ";
        command.Commands = (await connection.QueryAsync<CommandOut>(commandsSql, new{ id })).ToList();
        command.Commands.ForEach(x => x.Description = x.Type.ToCommandDescription());

        return command;
    }
}
