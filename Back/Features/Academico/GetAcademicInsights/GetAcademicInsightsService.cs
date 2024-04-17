using Dapper;
using Npgsql;

namespace Syki.Back.Features.Academico.GetAcademicInsights;

public class GetAcademicInsightsService(DatabaseSettings settings)
{
    public async Task<AcademicInsightsOut> Get(Guid institutionId)
    {
        using var connection = new NpgsqlConnection(settings.ConnectionString);

        const string sql = @"
            SELECT
                (SELECT COUNT(1) FROM syki.campi WHERE institution_id = i.id)         AS campus,
                (SELECT COUNT(1) FROM syki.cursos WHERE institution_id = i.id)        AS cursos,
                (SELECT COUNT(1) FROM syki.disciplinas WHERE institution_id = i.id)   AS disciplinas,
                (SELECT COUNT(1) FROM syki.grades WHERE institution_id = i.id)        AS grades,
                (SELECT COUNT(1) FROM syki.ofertas WHERE institution_id = i.id)       AS ofertas,
                (SELECT COUNT(1) FROM syki.turmas WHERE institution_id = i.id)        AS turmas,
                (SELECT COUNT(1) FROM syki.professores WHERE institution_id = i.id)   AS professores,
                (SELECT COUNT(1) FROM syki.alunos WHERE institution_id = i.id)        AS alunos,
                (SELECT COUNT(1) FROM syki.notifications WHERE institution_id = i.id) AS notifications
            FROM
            	syki.institutions i
            WHERE
            	i.id = @Id
        ";

        var parameters = new { Id = institutionId };

        return await connection.QueryFirstAsync<AcademicInsightsOut>(sql, parameters);
    }
}
