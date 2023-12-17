using Syki.Shared;
using Syki.Back.Domain;
using Syki.Back.Database;
using Microsoft.EntityFrameworkCore;

namespace Syki.Back.Services;

public class TurmasService : ITurmasService
{
    private readonly SykiDbContext _ctx;
    public TurmasService(SykiDbContext ctx) => _ctx = ctx;

    public async Task<TurmaOut> Create(Guid faculdadeId, TurmaIn data)
    {
        var turma = new Turma(
            faculdadeId,
            data.DisciplinaId,
            data.ProfessorId,
            data.Periodo
        );
        _ctx.Turmas.Add(turma);

        var vinculos = data.Ofertas.ConvertAll(x =>
            new OfertaTurma { OfertaId = x, TurmaId = turma.Id });
        _ctx.AddRange(vinculos);

        await _ctx.SaveChangesAsync();

        turma = await _ctx.Turmas.AsNoTracking()
            .Include(t => t.Disciplina)
            .Include(t => t.Professor)
            .FirstAsync(x => x.Id == turma.Id);

        return turma.ToOut();
    }

    public async Task<List<TurmaOut>> GetAll(Guid faculdadeId)
    {
        var turmas = await _ctx.Turmas.AsNoTracking()
            .Include(t => t.Disciplina)
            .Include(t => t.Professor)
            .Where(c => c.FaculdadeId == faculdadeId)
            .ToListAsync();

        return turmas.ConvertAll(t => t.ToOut());
    }
}
