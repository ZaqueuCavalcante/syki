using Dapper;
using Npgsql;

namespace Syki.Back.Features.Adm.GetCommands;

public class GetCommandsService(DatabaseSettings settings) : IAdmService
{
    public async Task<List<CommandTableOut>> Get(CommandTableFilterIn filters)
    {
        await using var dataSource = NpgsqlDataSource.Create(settings.ConnectionString);
        await using var connection = await dataSource.OpenConnectionAsync();

        const string sql = @"
            SELECT
                id,
                type,
                status,
                created_at,
                processed_at,
                duration
            FROM
                syki.commands
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
            filters.Type,
            filters.InstitutionId,
            Status = filters.Status?.ToString(),
        };

        var commands = (await connection.QueryAsync<CommandTableOut>(sql, parameters)).ToList();

        commands.ForEach(x => x.Description = x.Type.ToCommandDescription());

        return commands;
    }
}
