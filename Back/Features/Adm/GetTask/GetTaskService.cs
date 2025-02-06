using Dapper;
using Npgsql;

namespace Syki.Back.Features.Adm.GetTask;

public class GetTaskService(DatabaseSettings settings) : IAdmService
{
    public async Task<SykiTaskOut> Get(Guid id)
    {
        await using var dataSource = NpgsqlDataSource.Create(settings.ConnectionString);
        await using var connection = await dataSource.OpenConnectionAsync();

        const string sql = @"
            SELECT *
            FROM syki.tasks
            WHERE id = @Id
        ";
        var task = await connection.QueryFirstOrDefaultAsync<SykiTaskOut>(sql, new{ id }) ?? new();
        task.Description = task.Type.ToSykiTaskDescription();

        const string tasksSql = @"
            SELECT *
            FROM syki.tasks
            WHERE parent_id = @Id
        ";
        task.Tasks = (await connection.QueryAsync<SykiTaskOut>(tasksSql, new{ id })).ToList();
        task.Tasks.ForEach(x => x.Description = x.Type.ToSykiTaskDescription());

        return task;
    }
}
