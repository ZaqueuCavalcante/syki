namespace Syki.Back.Features.Academic.GetCampusStudents;

public class GetCampusStudentsService(SykiDbContext ctx) : IAcademicService
{
    public async Task<List<GetCampusStudentsOut>> Get(Guid institutionId, Guid campusId)
    {
        FormattableString sql = $@"
            SELECT
                s.id,
                s.name
            FROM
                syki.students s
            INNER JOIN
                syki.course_offerings co ON co.id = s.course_offering_id
            WHERE
                s.institution_id = {institutionId}
                    AND
                co.campus_id = {campusId}
            GROUP BY
                s.id,
                s.name
            ORDER BY
                s.name
        ";

        return await ctx.Database.SqlQuery<GetCampusStudentsOut>(sql).ToListAsync();
    }
}
