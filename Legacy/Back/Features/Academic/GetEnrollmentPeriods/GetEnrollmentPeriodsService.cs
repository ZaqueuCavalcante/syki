namespace Estud.Back.Features.Academic.GetEnrollmentPeriods;

public class GetEnrollmentPeriodsService(EstudDbContext ctx, HybridCache cache) : IEstudService
{
    public async Task<List<EnrollmentPeriodOut>> Get(Guid institutionId)
    {
        return await cache.GetOrCreateAsync(
            key: $"enrollmentPeriods:{institutionId}",
            state: (ctx, institutionId),
            factory: async (state, _) =>
            {
                var data = await state.ctx.EnrollmentPeriods.AsNoTracking()
                    .Where(c => c.InstitutionId == state.institutionId)
                    .OrderBy(p => p.Id)
                    .ToListAsync();
                return data.ConvertAll(c => c.ToOut());
            }
        );
    }
}
