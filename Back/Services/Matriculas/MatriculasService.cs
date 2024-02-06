using Syki.Shared;
using Syki.Back.Domain;
using Syki.Back.Database;
using Microsoft.EntityFrameworkCore;

namespace Syki.Back.Services;

public class MatriculasService : IMatriculasService
{
    private readonly SykiDbContext _ctx;
    public MatriculasService(SykiDbContext ctx) => _ctx = ctx;

    public async Task<PeriodoDeMatriculaOut> CreatePeriodoDeMatricula(Guid faculdadeId, PeriodoDeMatriculaIn data)
    {
        var periodo = new PeriodoDeMatricula(data.Id, faculdadeId, data.Start, data.End);

        _ctx.Add(periodo);
        await _ctx.SaveChangesAsync();

        return periodo.ToOut();
    }

    public async Task<List<PeriodoDeMatriculaOut>> GetPeriodosDeMatricula(Guid faculdadeId)
    {
        var periodos = await _ctx.PeriodosDeMatricula
            .Where(c => c.FaculdadeId == faculdadeId)
            .ToListAsync();

        return periodos.ConvertAll(p => p.ToOut());
    }
}
