namespace Syki.Back.Features.Academic.GetAcademicPeriods;

public class GetAcademicPeriodsService(SykiDbContext ctx, HybridCache cache) : IAcademicService
{
    public async Task<List<AcademicPeriodOut>> Get(Guid institutionId)
    {
        return await cache.GetOrCreateAsync(
            key: $"academicPeriods:{institutionId}",
            state: (ctx, institutionId),
            factory: async (state, _) =>
            {
                var data = await state.ctx.AcademicPeriods.AsNoTracking()
                    .Where(p => p.InstitutionId == state.institutionId)
                    .OrderBy(p => p.Id)
                    .ToListAsync();
                return data.ConvertAll(p => p.ToOut());
            }
        );
    }
}
