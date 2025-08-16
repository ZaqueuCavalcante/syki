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
                COALESCE(tc.total, 0) AS campi,
                COALESCE(td.total, 0) AS disciplines
            FROM
                syki.teachers t
            INNER JOIN
                syki.users u ON u.id = t.id
            LEFT JOIN (
                SELECT syki_teacher_id, COUNT(1) AS total
                FROM syki.teachers__campi
                GROUP BY syki_teacher_id
            ) tc ON tc.syki_teacher_id = t.id
            LEFT JOIN (
                SELECT syki_teacher_id, COUNT(1) AS total
                FROM syki.teachers__disciplines
                GROUP BY syki_teacher_id
            ) td ON td.syki_teacher_id = t.id
            WHERE
                u.institution_id = {institutionId}
            ORDER BY
                t.name
        ";

        return await ctx.Database.SqlQuery<TeacherOut>(sql).ToListAsync();
    }
}
