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

        const string retriesSql = @"
            SELECT *
            FROM syki.commands
            WHERE original_id = @Id
        ";
        command.Retries = (await connection.QueryAsync<CommandOut>(retriesSql, new{ id })).ToList();
        command.Retries.ForEach(x => x.Description = x.Type.ToCommandDescription());

        const string subcommandsSql = @"
            SELECT *
            FROM syki.commands
            WHERE parent_id = @Id
        ";
        command.Subcommands = (await connection.QueryAsync<CommandOut>(subcommandsSql, new{ id })).ToList();
        command.Subcommands.ForEach(x => x.Description = x.Type.ToCommandDescription());

        return command;
    }
}
