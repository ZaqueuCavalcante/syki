using Dapper;
using Npgsql;

namespace Syki.Back.GetProfessorInsights;

public class GetProfessorInsightsService(DatabaseSettings settings)
{
    public async Task<IndexProfessorOut> Get(Guid institutionId, Guid id)
    {
        using var connection = new NpgsqlConnection(settings.ConnectionString);

        const string sql = @"
            SELECT
                (SELECT COUNT(1) FROM syki.turmas WHERE faculdade_id = f.id AND professor_id = @Id) AS turmas,
                (
                    SELECT COUNT(1) FROM syki.turmas__alunos ta WHERE ta.turma_id IN
                        (SELECT id FROM syki.turmas WHERE faculdade_id = f.id AND professor_id = @Id)
                ) AS alunos
            FROM
            	syki.faculdades f
            WHERE
            	f.id = @InstitutionId
        ";

        var parameters = new { InstitutionId = institutionId, Id = id };

        var data = await connection.QueryFirstAsync<IndexProfessorOut>(sql, parameters);
        
        return data;
    }
}
