using Dapper;
using Npgsql;

namespace Syki.Back.Features.Academic.GetAcademicInsights;

public class GetAcademicInsightsService(DatabaseSettings settings)
{
    public async Task<AcademicInsightsOut> Get(Guid institutionId)
    {
        using var connection = new NpgsqlConnection(settings.ConnectionString);

        const string sql = @"
            SELECT
                (SELECT COUNT(1) FROM syki.campi WHERE institution_id = i.id)              AS campus,
                (SELECT COUNT(1) FROM syki.courses WHERE institution_id = i.id)            AS courses,
                (SELECT COUNT(1) FROM syki.disciplines WHERE institution_id = i.id)        AS disciplines,
                (SELECT COUNT(1) FROM syki.course-curriculums WHERE institution_id = i.id) AS course-curriculums,
                (SELECT COUNT(1) FROM syki.course-offerings WHERE institution_id = i.id)   AS course-offerings,
                (SELECT COUNT(1) FROM syki.classes WHERE institution_id = i.id)            AS classes,
                (SELECT COUNT(1) FROM syki.teachers WHERE institution_id = i.id)           AS teachers,
                (SELECT COUNT(1) FROM syki.students WHERE institution_id = i.id)           AS students,
                (SELECT COUNT(1) FROM syki.notifications WHERE institution_id = i.id)      AS notifications
            FROM
            	syki.institutions i
            WHERE
            	i.id = @Id
        ";

        var parameters = new { Id = institutionId };

        return await connection.QueryFirstAsync<AcademicInsightsOut>(sql, parameters);
    }
}
