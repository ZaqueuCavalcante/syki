using Dapper;
using Npgsql;

namespace Syki.Back.GetAlunoInsights;

public class GetAlunoInsightsService(DatabaseSettings settings)
{
    public async Task<IndexAlunoOut> Get(Guid id)
    {
        using var connection = new NpgsqlConnection(settings.ConnectionString);

        const string sql = @"
            SELECT COUNT(1)
            FROM syki.users
            WHERE id = @Id
        ";

        var parameters = new { Id = id };

        var data = await connection.QueryFirstAsync<IndexAlunoOut>(sql, parameters);

        data = new IndexAlunoOut
        {
            DisciplinasConcluidas = 5,
            DisciplinasTotal = 78,
            Media = 7.9M,
            CR = 6.5M,
        };
        
        return data;
    }
}
