using Dapper;
using Npgsql;

namespace Estud.Back.Features.Adm.GetAdmInsights;

public class GetAdmInsightsService(NpgsqlDataSource dataSource) : IEstudService
{
    public async Task<AdmInsightsOut> Get()
    {
        await using var connection = await dataSource.OpenConnectionAsync();

        const string sql = @"
            SELECT
                COUNT(1)-1                                     AS institutions,
                (SELECT COUNT(1)-1 FROM estud.users)            AS users,
                (SELECT COUNT(1) FROM estud.campi)              AS campi,
                (SELECT COUNT(1) FROM estud.courses)            AS courses,
                (SELECT COUNT(1) FROM estud.disciplines)        AS disciplines,
                (SELECT COUNT(1) FROM estud.course_curriculums) AS course_curriculums,
                (SELECT COUNT(1) FROM estud.course_offerings)   AS course_offerings,
                (SELECT COUNT(1) FROM estud.teachers)           AS teachers,
                (SELECT COUNT(1) FROM estud.students)           AS students
            FROM
            	estud.institutions
        ";

        return await connection.QueryFirstAsync<AdmInsightsOut>(sql);
    }
}
