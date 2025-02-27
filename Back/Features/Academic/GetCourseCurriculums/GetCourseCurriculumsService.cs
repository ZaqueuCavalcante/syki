namespace Syki.Back.Features.Academic.GetCourseCurriculums;

public class GetCourseCurriculumsService(SykiDbContext ctx, HybridCache cache) : IAcademicService
{
    public async Task<List<CourseCurriculumOut>> Get(Guid institutionId)
    {
        var courseCurriculums = await cache.GetOrCreateAsync(
            key: $"courseCurriculums:{institutionId}",
            state: (ctx, institutionId),
            factory: async (state, _) =>
            {
                var data = await state.ctx.CourseCurriculums.AsNoTracking()
                    .Where(g => g.InstitutionId == state.institutionId)
                    .Include(g => g.Course)
                    .Include(g => g.Disciplines)
                    .Include(g => g.Links)
                    .OrderBy(g => g.Name)
                    .ToListAsync();
                return data.ConvertAll(g => g.ToOut());
            }
        );

        return courseCurriculums;
    }
}
