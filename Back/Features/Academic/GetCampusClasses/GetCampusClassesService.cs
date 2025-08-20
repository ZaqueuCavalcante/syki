using Dapper;
using Npgsql;

namespace Syki.Back.Features.Academic.GetCampusClasses;

public class GetCampusClassesService(SykiDbContext ctx, NpgsqlDataSource dataSource) : IAcademicService
{
    public async Task<List<GetCampusClassesOut>> Get(Guid institutionId, Guid campusId)
    {
        await using var connection = await dataSource.OpenConnectionAsync();

        var period = await ctx.GetCurrentAcademicPeriod(institutionId);

        const string sql = @"
            SELECT
                c.id,
                c.discipline_id,
                d.name AS discipline,
                c.vacancies,
                c.workload,
                c.status,
                c.teacher_id,
                t.name AS teacher
            FROM
                syki.classes c
            INNER JOIN
                syki.disciplines d ON d.id = c.discipline_id
            LEFT JOIN
            	syki.teachers t ON t.id = c.teacher_id
            WHERE
                c.institution_id = @InstitutionId
                    AND
                c.campus_id = @CampusId
                	AND
                c.period_id = @PeriodId
            ORDER BY
                d.name
        ";

        var parameters = new
        {
            campusId,
            institutionId,
            PeriodId = period.Id
        };

        return (await connection.QueryAsync<GetCampusClassesOut>(sql, parameters)).ToList();
    }
}
