namespace Syki.Back.Features.Academic.GetCampi;

public class GetCampiService(SykiDbContext ctx, HybridCache cache) : IAcademicService
{
    public async Task<List<CampusOut>> Get(Guid institutionId)
    {
        var campi = await cache.GetOrCreateAsync(
            key: $"campi:{institutionId}",
            state: (ctx, institutionId),
            factory: async (state, _) =>
            {
                var data = await state.ctx.Campi.AsNoTracking()
                    .Where(c => c.InstitutionId == state.institutionId)
                    .ToListAsync();
                return data.ConvertAll(c => c.ToOut());
            }
        );

        return campi;
    }
}
