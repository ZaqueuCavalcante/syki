using Dapper;
using Npgsql;

namespace Syki.Back.Features.Adm.GetInsights;

public class GetAdmInsightsService(DatabaseSettings settings)
{
    public async Task<IndexAdmOut> Get()
    {
        using var connection = new NpgsqlConnection(settings.ConnectionString);

        const string sql = @"
            SELECT
                COUNT(1)-1                              AS institutions,
                (SELECT COUNT(1)-1 FROM syki.users)     AS users,
                (SELECT COUNT(1) FROM syki.campi)       AS campus,
                (SELECT COUNT(1) FROM syki.cursos)      AS cursos,
                (SELECT COUNT(1) FROM syki.disciplines) AS disciplines,
                (SELECT COUNT(1) FROM syki.grades)      AS grades,
                (SELECT COUNT(1) FROM syki.ofertas)     AS ofertas,
                (SELECT COUNT(1) FROM syki.professores) AS professores,
                (SELECT COUNT(1) FROM syki.alunos)      AS alunos
            FROM
            	syki.institutions
        ";

        return await connection.QueryFirstAsync<IndexAdmOut>(sql);
    }
}
