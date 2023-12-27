using Syki.Shared;
using Syki.Back.Domain;
using Syki.Back.Database;
using Microsoft.EntityFrameworkCore;

namespace Syki.Back.Services;

public class PeriodosService : IPeriodosService
{
    private readonly SykiDbContext _ctx;
    public PeriodosService(SykiDbContext ctx) => _ctx = ctx;

    public async Task<PeriodoOut> Create(Guid faculdadeId, PeriodoIn data)
    {
        var periodo = new Periodo(data.Id, faculdadeId, data.Start, data.End);

        await _ctx.Periodos.AddAsync(periodo);
        await _ctx.SaveChangesAsync();

        return periodo.ToOut();
    }

    public async Task<List<PeriodoOut>> GetAll(Guid faculdadeId)
    {
        var periodos = await _ctx.Periodos
            .Where(c => c.FaculdadeId == faculdadeId)
            .ToListAsync();

        return periodos.ConvertAll(p => p.ToOut());
    }
}
