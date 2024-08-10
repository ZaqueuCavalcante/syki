namespace Syki.Back.Features.Academic.GetCampi;

public class GetCampiService(SykiDbContext ctx) : IAcademicService
{
    public async Task<List<CampusOut>> Get(Guid institutionId)
    {
        var campi = await ctx.Campi.AsNoTracking()
            .Where(c => c.InstitutionId == institutionId)
            .ToListAsync();
        
        return campi.ConvertAll(p => p.ToOut());
    }
}
