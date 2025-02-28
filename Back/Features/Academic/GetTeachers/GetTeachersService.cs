namespace Syki.Back.Features.Academic.GetTeachers;

public class GetTeachersService(SykiDbContext ctx, HybridCache cache) : IAcademicService
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

        return await cache.GetOrCreateAsync(
            key: $"teachers:{institutionId}",
            state: (ctx, sql),
            factory: async (state, _) =>
            {
                return await state.ctx.Database.SqlQuery<TeacherOut>(state.sql).ToListAsync();
            }
        );
    }
}
