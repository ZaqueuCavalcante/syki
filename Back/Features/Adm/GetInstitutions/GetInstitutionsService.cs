namespace Syki.Back.Features.Adm.GetInstitutions;

public class GetInstitutionsService(SykiDbContext ctx, HybridCache cache) : IAdmService
{
    public async Task<List<InstitutionOut>> Get()
    {
        return await cache.GetOrCreateAsync(
            key: "institutions",
            state: ctx,
            factory: async (state, _) =>
            {
                var data = await state.Institutions
                    .Include(x => x.Configs)
                    .Where(i => i.Id != Guid.Empty)
                    .OrderByDescending(x => x.CreatedAt)
                    .ToListAsync();
                return data.ConvertAll(i => i.ToOut());
            }
        );
    }
}
