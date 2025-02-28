namespace Syki.Back.Features.Academic.GetCourses;

public class GetCoursesService(SykiDbContext ctx, HybridCache cache) : IAcademicService
{
    public async Task<List<CourseOut>> Get(Guid institutionId)
    {
        var courses = await cache.GetOrCreateAsync(
            key: $"courses:{institutionId}",
            state: (ctx, institutionId),
            factory: async (state, _) =>
            {
                var data = await state.ctx.Courses
                    .Where(c => c.InstitutionId == state.institutionId)
                    .OrderBy(c => c.Name)
                    .ToListAsync();
                return data.ConvertAll(c => c.ToOut());
            }
        );

        return courses;
    }
}
