using Dapper;
using Npgsql;

namespace Syki.Back.GetProfessorInsights;

public class GetProfessorInsightsService(DatabaseSettings settings)
{
    public async Task<IndexProfessorOut> Get(Guid id)
    {
        using var connection = new NpgsqlConnection(settings.ConnectionString);

        const string sql = @"
            SELECT COUNT(1)
            FROM syki.users
            WHERE id = @Id
        ";

        var parameters = new { Id = id };

        var data = await connection.QueryFirstAsync<IndexProfessorOut>(sql, parameters);

        data = new IndexProfessorOut
        {
            Turmas = 5,
            Alunos = 78,
            Aulas = 154,
            Media = 7.5M,
        };
        
        return data;
    }
}
