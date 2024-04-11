using Dapper;
using Npgsql;

namespace Syki.Back.GetAlunoInsights;

public class GetAlunoInsightsService(DatabaseSettings settings)
{
    public async Task<IndexAlunoOut> Get(Guid id)
    {
        using var connection = new NpgsqlConnection(settings.ConnectionString);

        const string sql = @"
            SELECT COUNT(1) AS DisciplinasTotal
            FROM syki.grades__disciplinas
            WHERE grade_id =
            (
                SELECT o.grade_id
                FROM syki.ofertas o
                WHERE id = (SELECT oferta_id FROM syki.alunos a WHERE a.id = @Id)
            )
        ";

        var parameters = new { Id = id };

        var data = await connection.QueryFirstAsync<IndexAlunoOut>(sql, parameters);

        return data;
    }
}
