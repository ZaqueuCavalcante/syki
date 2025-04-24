namespace Syki.Back.Features.Academic.GetCourses;

public class GetCoursesService(SykiDbContext ctx, HybridCache cache) : IAcademicService
{
    public async Task<List<CourseOut>> Get(Guid institutionId)
    {
        return await cache.GetOrCreateAsync(
            key: $"courses:{institutionId}",
            state: (ctx, institutionId),
            factory: async (state, cancellationToken) =>
            {
                var courses = await state.ctx.Courses.AsNoTracking()
                    .Where(c => c.InstitutionId == state.institutionId)
                    .OrderBy(c => c.Name)
                    .ToListAsync(cancellationToken);
                return courses.ConvertAll(c => c.ToOut());
            }
        );
    }
}
