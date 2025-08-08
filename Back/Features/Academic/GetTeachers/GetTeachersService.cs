namespace Syki.Back.Features.Academic.GetTeachers;

public class GetTeachersService(SykiDbContext ctx) : IAcademicService
{
    public async Task<List<TeacherOut>> Get(Guid institutionId)
    {
        FormattableString sql = $@"
            SELECT
                t.id,
                t.name,
                u.email,
                COUNT(td.syki_teacher_id) AS disciplines
            FROM
                syki.teachers t
            INNER JOIN
                syki.users u ON u.id = t.id
            LEFT JOIN
                syki.teachers__disciplines td ON td.syki_teacher_id = t.id
            WHERE
                u.institution_id = {institutionId}
            GROUP BY
                t.id,
                t.name,
                u.email
            ORDER BY
                t.name
        ";

        return await ctx.Database.SqlQuery<TeacherOut>(sql).ToListAsync();
    }
}
