using Dapper;
using Npgsql;

namespace Syki.Daemon.Tasks;

public class SykiTasksProcessedCleaner(IConfiguration configuration)
{
    public async Task Run()
    {
        await using var connection = new NpgsqlConnection(configuration.Database().ConnectionString);

        const string sql = @"
            WITH success_tasks AS (
                DELETE FROM syki.tasks
                WHERE status = 'Success'
                RETURNING *
            )
            INSERT INTO syki.processed_tasks
            SELECT * FROM success_tasks;
        ";

        await connection.ExecuteScalarAsync(sql);
    }
}
