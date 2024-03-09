namespace Syki.Back.GetCampi;

public class GetCampiService(SykiDbContext ctx)
{
    public async Task<List<CampusOut>> Get(Guid institutionId)
    {
        var campi = await ctx.Campi.AsNoTracking()
            .Where(c => c.InstitutionId == institutionId)
            .ToListAsync();
        
        return campi.ConvertAll(p => p.ToOut());
    }
}
