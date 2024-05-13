using Dapper;
using Npgsql;

namespace Syki.Back.Features.Student.GetStudentInsights;

public class GetStudentInsightsService(DatabaseSettings settings)
{
    public async Task<IndexStudentOut> Get(Guid id)
    {
        using var connection = new NpgsqlConnection(settings.ConnectionString);

        const string sql = @"
            SELECT COUNT(1) AS DisciplinesTotal
            FROM syki.grades__disciplines
            WHERE grade_id =
            (
                SELECT o.grade_id
                FROM syki.ofertas o
                WHERE id = (SELECT oferta_id FROM syki.alunos a WHERE a.id = @Id)
            )
        ";

        var parameters = new { Id = id };

        var data = await connection.QueryFirstAsync<IndexStudentOut>(sql, parameters);

        return data;
    }
}
