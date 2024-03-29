using Dapper;
using Npgsql;

namespace Syki.Back.GetAdmInsights;

public class GetAdmInsightsService(DatabaseSettings settings)
{
    public async Task<IndexAdmOut> Get()
    {
        using var connection = new NpgsqlConnection(settings.ConnectionString);

        const string sql = @"
            SELECT
                COUNT(1)-1 AS faculdades,
                (SELECT COUNT(1)-1 FROM syki.users) AS users,
                (SELECT COUNT(1) FROM syki.campi) AS campus,
                (SELECT COUNT(1) FROM syki.cursos) AS cursos,
                (SELECT COUNT(1) FROM syki.disciplinas) AS disciplinas,
                (SELECT COUNT(1) FROM syki.grades) AS grades,
                (SELECT COUNT(1) FROM syki.ofertas) AS ofertas,
                (SELECT COUNT(1) FROM syki.professores) AS professores,
                (SELECT COUNT(1) FROM syki.alunos) AS alunos
            FROM
            	syki.faculdades
        ";

        return await connection.QueryFirstAsync<IndexAdmOut>(sql);
    }
}
