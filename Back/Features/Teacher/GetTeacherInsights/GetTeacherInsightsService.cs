using Dapper;
using Npgsql;

namespace Syki.Back.Features.Teacher.GetTeacherInsights;

public class GetTeacherInsightsService(DatabaseSettings settings)
{
    public async Task<IndexTeacherOut> Get(Guid institutionId, Guid id)
    {
        using var connection = new NpgsqlConnection(settings.ConnectionString);

        const string sql = @"
            SELECT
                (SELECT COUNT(1) FROM syki.turmas WHERE institution_id = i.id AND professor_id = @Id) AS turmas,
                (
                    SELECT COUNT(1) FROM syki.turmas__alunos ta WHERE ta.turma_id IN
                        (SELECT id FROM syki.turmas WHERE institution_id = i.id AND professor_id = @Id)
                ) AS alunos
            FROM
            	syki.institutions i
            WHERE
            	i.id = @InstitutionId
        ";

        var parameters = new { InstitutionId = institutionId, Id = id };

        var data = await connection.QueryFirstAsync<IndexTeacherOut>(sql, parameters);
        
        return data;
    }
}
