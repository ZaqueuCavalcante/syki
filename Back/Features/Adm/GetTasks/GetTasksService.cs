using Dapper;
using Npgsql;

namespace Syki.Back.Features.Adm.GetTasks;

public class GetTasksService(DatabaseSettings settings) : IAdmService
{
    public async Task<List<SykiTaskTableOut>> Get(SykiTaskTableFilterIn filters)
    {
        using var connection = new NpgsqlConnection(settings.ConnectionString);

        const string sql = @"
            SELECT
                id,
                type,
                status,
                created_at,
                processed_at,
                duration
            FROM
                syki.tasks
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

        var tasks = (await connection.QueryAsync<SykiTaskTableOut>(sql, parameters)).ToList();

        tasks.ForEach(x => x.Description = x.Type.ToSykiTaskDescription());

        return tasks;
    }
}
