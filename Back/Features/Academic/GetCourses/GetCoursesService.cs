namespace Syki.Back.Features.Academic.GetCourses;

public class GetCoursesService(SykiDbContext ctx, HybridCache cache) : IAcademicService
{
    public async Task<List<CreateCourseOut>> Get(Guid institutionId)
    {
        return await cache.GetOrCreateAsync(
            key: $"courses:{institutionId}",
            state: (ctx, institutionId),
            factory: async (state, _) =>
            {
                var courses = await state.ctx.GetCourses(state.institutionId);
                return courses.ConvertAll(c => c.ToOut());
            }
        );
    }
}
