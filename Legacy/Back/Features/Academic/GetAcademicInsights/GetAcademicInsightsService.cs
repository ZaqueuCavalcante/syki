using Dapper;
using Npgsql;

namespace Estud.Back.Features.Academic.GetAcademicInsights;

public class GetAcademicInsightsService(NpgsqlDataSource dataSource) : IEstudService
{
    public async Task<AcademicInsightsOut> Get(Guid institutionId)
    {
        await using var connection = await dataSource.OpenConnectionAsync();

        const string sql = @"
            SELECT
                (SELECT COUNT(1) FROM estud.campi WHERE institution_id = i.id)                 AS campus,
                (SELECT COUNT(1) FROM estud.courses WHERE institution_id = i.id)               AS courses,
                (SELECT COUNT(1) FROM estud.disciplines WHERE institution_id = i.id)           AS disciplines,
                (SELECT COUNT(1) FROM estud.course_curriculums WHERE institution_id = i.id)    AS course_curriculums,
                (SELECT COUNT(1) FROM estud.course_offerings WHERE institution_id = i.id)      AS course_offerings,
                (SELECT COUNT(1) FROM estud.classes WHERE institution_id = i.id)               AS classes,
                (SELECT COUNT(1) FROM estud.teachers WHERE institution_id = i.id)              AS teachers,
                (SELECT COUNT(1) FROM estud.students WHERE institution_id = i.id)              AS students,
                (SELECT COUNT(1) FROM estud.notifications WHERE institution_id = i.id)         AS notifications,
                (SELECT COUNT(1) FROM estud.webhook_subscriptions WHERE institution_id = i.id) AS webhooks
            FROM
            	estud.institutions i
            WHERE
            	i.id = @Id
        ";

        var parameters = new { Id = institutionId };

        return await connection.QueryFirstAsync<AcademicInsightsOut>(sql, parameters);
    }
}
