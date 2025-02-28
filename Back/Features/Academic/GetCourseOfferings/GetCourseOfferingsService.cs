namespace Syki.Back.Features.Academic.GetCourseOfferings;

public class GetCourseOfferingsService(SykiDbContext ctx, HybridCache cache) : IAcademicService
{
    public async Task<List<CourseOfferingOut>> Get(Guid institutionId)
    {
        return await cache.GetOrCreateAsync(
            key: $"courseOfferings:{institutionId}",
            state: (ctx, institutionId),
            factory: async (state, _) =>
            {
                var data = await state.ctx.CourseOfferings.AsNoTracking()
                    .Include(x => x.Campus)
                    .Include(x => x.Course)
                    .Include(x => x.CourseCurriculum)
                    .Where(c => c.InstitutionId == state.institutionId)
                    .ToListAsync();
                return data.ConvertAll(c => c.ToOut());
            }
        );
    }
}
