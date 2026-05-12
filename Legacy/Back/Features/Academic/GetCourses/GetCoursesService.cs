namespace Syki.Back.Features.Academic.GetCourses;

public class GetCoursesService(SykiDbContext ctx, HybridCache cache) : ISykiService
{
    public async Task<GetCoursesOut> Get()
    {
        return await cache.GetOrCreateAsync(
            key: $"courses:{ctx.InstitutionId}",
            state: ctx,
            factory: async (state, _) =>
            {
                var courses = await state.GetCourses(state.InstitutionId);
                return new GetCoursesOut
                {
                    Total = courses.Count,
                    Items = courses.ConvertAll(c => c.ToGetCoursesItemOut())
                };
            }
        );
    }
}
