using Dapper;
using Npgsql;

namespace Syki.Back.GetAcademicInsights;

public class GetAcademicInsightsService(DatabaseSettings settings)
{
    public async Task<IndexAcademicoOut> Get(Guid institutionId)
    {
        using var connection = new NpgsqlConnection(settings.ConnectionString);

        const string sql = @"
            SELECT
                (SELECT COUNT(1) FROM syki.campi WHERE institution_id = f.id) AS campus,
                (SELECT COUNT(1) FROM syki.cursos WHERE faculdade_id = f.id) AS cursos,
                (SELECT COUNT(1) FROM syki.disciplinas WHERE faculdade_id = f.id) AS disciplinas,
                (SELECT COUNT(1) FROM syki.grades WHERE faculdade_id = f.id) AS grades,
                (SELECT COUNT(1) FROM syki.ofertas WHERE faculdade_id = f.id) AS ofertas,
                (SELECT COUNT(1) FROM syki.turmas WHERE faculdade_id = f.id) AS turmas,
                (SELECT COUNT(1) FROM syki.professores WHERE faculdade_id = f.id) AS professores,
                (SELECT COUNT(1) FROM syki.alunos WHERE institution_id = f.id) AS alunos,
                (SELECT COUNT(1) FROM syki.notifications WHERE faculdade_id = f.id) AS notifications
            FROM
            	syki.faculdades f
            WHERE
            	f.id = @Facul
        ";

        var parameters = new { Facul = institutionId };

        return await connection.QueryFirstAsync<IndexAcademicoOut>(sql, parameters);
    }
}
