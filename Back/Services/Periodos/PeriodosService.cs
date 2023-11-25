using Syki.Shared;
using Syki.Back.Database;
using Microsoft.EntityFrameworkCore;

namespace Syki.Back.Services;

public class PeriodosService : IPeriodosService
{
    private readonly SykiDbContext _ctx;
    public PeriodosService(SykiDbContext ctx) => _ctx = ctx;

    public async Task<List<PeriodoOut>> GetAll(Guid faculdadeId)
    {
        var periodos = await _ctx.Periodos
            .Where(c => c.FaculdadeId == faculdadeId)
            .ToListAsync();

        return periodos.ConvertAll(p => p.ToOut());
    }
}
