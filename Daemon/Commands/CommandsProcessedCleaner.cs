using Dapper;
using Npgsql;

namespace Syki.Daemon.Commands;

public class CommandsProcessedCleaner(IConfiguration configuration)
{
    public async Task Run()
    {
        await using var connection = new NpgsqlConnection(configuration.Database().ConnectionString);

        const string sql = @"
            WITH success_commands AS (
                DELETE FROM syki.commands
                WHERE status = 'Success'
                RETURNING *
            )
            INSERT INTO syki.processed_commands
            SELECT * FROM success_commands;
        ";

        await connection.ExecuteScalarAsync(sql);
    }
}
