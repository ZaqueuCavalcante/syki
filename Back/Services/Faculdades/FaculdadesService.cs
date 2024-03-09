namespace Syki.Back.Services;

public class FaculdadesService(SykiDbContext ctx) : IFaculdadesService
{
    public async Task<FaculdadeOut> Create(FaculdadeIn data)
    {
        var faculdade = new Faculdade(data.Nome);
        ctx.Add(faculdade);

        await ctx.SaveChangesAsync();

        return faculdade.ToOut();
    }

    public async Task<List<FaculdadeOut>> GetAll()
    {
        var faculdades = await ctx.Institutions
            .Where(x => x.Id != Guid.Empty)
            .ToListAsync();

        return faculdades.ConvertAll(f => f.ToOut());
    }
}
