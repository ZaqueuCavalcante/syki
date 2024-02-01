using Syki.Shared;
using Syki.Back.Tasks;
using Syki.Back.Domain;
using Syki.Back.Database;
using Microsoft.EntityFrameworkCore;

namespace Syki.Back.Services;

public class FaculdadesService : IFaculdadesService
{
    private readonly SykiDbContext _ctx;
    public FaculdadesService(SykiDbContext ctx) => _ctx = ctx;

    public async Task<FaculdadeOut> Create(FaculdadeIn data)
    {
        var faculdade = new Faculdade(data.Nome);
        _ctx.Add(faculdade);

        if (data.SeedData)
        {
            var task = new SykiTask(new SeedFaculdadeTestData { FaculdadeId = faculdade.Id });
            _ctx.Add(task);
        }

        await _ctx.SaveChangesAsync();

        return faculdade.ToOut();
    }

    public async Task<List<FaculdadeOut>> GetAll()
    {
        var faculdades = await _ctx.Faculdades
            .Where(x => x.Id != Guid.Empty)
            .ToListAsync();

        return faculdades.ConvertAll(f => f.ToOut());
    }
}
