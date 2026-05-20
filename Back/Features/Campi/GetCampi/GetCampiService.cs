namespace Syki.Back.Features.Campi.GetCampi;

public class GetCampiService(SykiDbContext ctx) : ISykiService
{
    public async Task<GetCampiOut> Get()
    {
        var institutionId = ctx.RequestUser.InstitutionId;

        var campi = await ctx.Campi.AsNoTracking()
            .Where(c => c.InstitutionId == institutionId)
            .OrderBy(c => c.Name)
            .ToListAsync();

        var items = campi.ConvertAll(x =>
        {
            return x.ToGetCampiItemOut(0, 0);
        });

        return new GetCampiOut() { Total = items.Count, Items = items };
    }
}
