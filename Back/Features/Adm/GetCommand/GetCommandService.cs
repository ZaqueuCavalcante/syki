using Dapper;
using Npgsql;

namespace Syki.Back.Features.Adm.GetCommand;

public class GetCommandService(NpgsqlDataSource dataSource) : IAdmService
{
    public async Task<CommandOut> Get(Guid id)
    {
        await using var connection = await dataSource.OpenConnectionAsync();

        const string sql = @"
            SELECT *
            FROM syki.commands
            WHERE id = @Id
        ";
        var command = await connection.QueryFirstOrDefaultAsync<CommandOut>(sql, new { id }) ?? new();
        command.Description = command.Type.ToCommandDescription();

        const string sourceBatchSql = @"
            SELECT id
            FROM syki.command_batches
            WHERE next_command_id = @Id
        ";
        command.SourceBatchId = await connection.QueryFirstOrDefaultAsync<Guid?>(sourceBatchSql, new { id });

        const string retriesSql = @"
            SELECT *
            FROM syki.commands
            WHERE original_id = @Id
        ";
        command.Retries = (await connection.QueryAsync<CommandOut>(retriesSql, new { id })).ToList();
        command.Retries.ForEach(x => x.Description = x.Type.ToCommandDescription());

        const string subcommandsSql = @"
            SELECT *
            FROM syki.commands
            WHERE parent_id = @Id
        ";
        command.Subcommands = (await connection.QueryAsync<CommandOut>(subcommandsSql, new { id })).ToList();
        command.Subcommands.ForEach(x => x.Description = x.Type.ToCommandDescription());

        const string bacthesSql = @"
            SELECT *
            FROM syki.command_batches
            WHERE source_command_id = @Id
        ";
        command.Batches = (await connection.QueryAsync<BatchOut>(bacthesSql, new { id })).ToList();

        return command;
    }
}
