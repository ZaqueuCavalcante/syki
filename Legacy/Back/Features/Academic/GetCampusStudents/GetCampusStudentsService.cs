namespace Estud.Back.Features.Academic.GetCampusStudents;

public class GetCampusStudentsService(EstudDbContext ctx) : IEstudService
{
    public async Task<List<GetCampusStudentsOut>> Get(Guid campusId)
    {
        FormattableString sql = $@"
            SELECT
                s.id,
                s.name,
                s.course_offering_id
            FROM
                estud.students s
            INNER JOIN
                estud.course_offerings co ON co.id = s.course_offering_id
            WHERE
                s.institution_id = {ctx.InstitutionId}
                    AND
                co.campus_id = {campusId}
            GROUP BY
                s.id, s.name
            ORDER BY
                s.name
        ";

        return await ctx.Database.SqlQuery<GetCampusStudentsOut>(sql).ToListAsync();
    }
}
