using Dapper;
using Npgsql;

namespace Syki.Back.Features.Adm.GetAdmInsights;

public class GetAdmInsightsService(DatabaseSettings settings) : IAdmService
{
    public async Task<AdmInsightsOut> Get()
    {
        using var connection = new NpgsqlConnection(settings.ConnectionString);

        const string sql = @"
            SELECT
                COUNT(1)-1                                     AS institutions,
                (SELECT COUNT(1)-1 FROM syki.users)            AS users,
                (SELECT COUNT(1) FROM syki.campi)              AS campi,
                (SELECT COUNT(1) FROM syki.courses)            AS courses,
                (SELECT COUNT(1) FROM syki.disciplines)        AS disciplines,
                (SELECT COUNT(1) FROM syki.course_curriculums) AS course_curriculums,
                (SELECT COUNT(1) FROM syki.course_offerings)   AS course_offerings,
                (SELECT COUNT(1) FROM syki.teachers)           AS teachers,
                (SELECT COUNT(1) FROM syki.students)           AS students
            FROM
            	syki.institutions
        ";

        return await connection.QueryFirstAsync<AdmInsightsOut>(sql);
    }
}
