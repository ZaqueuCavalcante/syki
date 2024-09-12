namespace Syki.Back.Features.Academic.GetTeachers;

public class GetTeachersService(SykiDbContext ctx) : IAcademicService
{
    public async Task<List<TeacherOut>> Get(Guid institutionId)
    {
        FormattableString sql = $@"
            SELECT
                t.id,
                t.name,
                u.email
            FROM
                syki.teachers t
            INNER JOIN
                syki.users u ON u.id = t.id
            WHERE
                u.institution_id = {institutionId}
            ORDER BY
                t.name
        ";

        var teachers = await ctx.Database.SqlQuery<TeacherOut>(sql).ToListAsync();

        return teachers;
    }
}
