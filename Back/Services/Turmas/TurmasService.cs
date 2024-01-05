using Syki.Shared;
using Syki.Back.Domain;
using Syki.Back.Database;
using Syki.Back.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Syki.Back.Services;

public class TurmasService : ITurmasService
{
    private readonly SykiDbContext _ctx;
    public TurmasService(SykiDbContext ctx) => _ctx = ctx;

    public async Task<TurmaOut> Create(Guid faculdadeId, TurmaIn data)
    {
        var disciplinaOk = await _ctx.Disciplinas
            .AnyAsync(c => c.FaculdadeId == faculdadeId && c.Id == data.DisciplinaId);
        if (!disciplinaOk)
            throw new DomainException(ExceptionMessages.DE0002);

        var professorOk = await _ctx.Professores
            .AnyAsync(p => p.FaculdadeId == faculdadeId && p.Id == data.ProfessorId);
        if (!professorOk)
            throw new DomainException(ExceptionMessages.DE0015);

        var periodoOk = await _ctx.Periodos
            .AnyAsync(p => p.FaculdadeId == faculdadeId && p.Id == data.Periodo);
        if (!periodoOk)
            throw new DomainException(ExceptionMessages.DE0003);

        var turma = new Turma(
            faculdadeId,
            data.DisciplinaId,
            data.ProfessorId,
            data.Periodo
        );

        _ctx.Turmas.Add(turma);
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
