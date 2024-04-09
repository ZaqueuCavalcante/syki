namespace Syki.Back.GetOfertas;

public class GetOfertasService(SykiDbContext ctx)
{
    public async Task<List<OfertaOut>> Get(Guid faculdadeId)
    {
        var ofertas = await ctx.Ofertas.AsNoTracking()
            .Include(x => x.Campus)
            .Include(x => x.Curso)
            .Include(x => x.Grade)
            .Where(c => c.FaculdadeId == faculdadeId)
            .ToListAsync();

        return ofertas.ConvertAll(o => o.ToOut());
    }
}
