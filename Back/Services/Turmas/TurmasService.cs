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
            .AnyAsync(x => x.FaculdadeId == faculdadeId && x.Id == data.DisciplinaId);
        if (!disciplinaOk)
            Throw.DE004.Now();

        var professorOk = await _ctx.Professores
            .AnyAsync(p => p.FaculdadeId == faculdadeId && p.Id == data.ProfessorId);
        if (!professorOk)
            Throw.DE018.Now();

        var periodoOk = await _ctx.AcademicPeriods
            .AnyAsync(p => p.InstitutionId == faculdadeId && p.Id == data.Periodo);
        if (!periodoOk)
            Throw.DE005.Now();

        var horarios = data.Horarios.ConvertAll(h => new Horario(h.Dia, h.Start, h.End));
        var turma = new Turma(
            faculdadeId,
            data.DisciplinaId,
            data.ProfessorId,
            data.Periodo,
            horarios
        );

        _ctx.Turmas.Add(turma);
        await _ctx.SaveChangesAsync();

        turma = await _ctx.Turmas.AsNoTracking()
            .Include(t => t.Disciplina)
            .Include(t => t.Professor)
            .Include(t => t.Horarios)
            .FirstAsync(x => x.Id == turma.Id);

        return turma.ToOut();
    }

    public async Task<List<TurmaOut>> GetAll(Guid faculdadeId)
    {
        var turmas = await _ctx.Turmas.AsNoTracking()
            .Include(t => t.Disciplina)
            .Include(t => t.Professor)
            .Include(t => t.Horarios)
            .Where(c => c.FaculdadeId == faculdadeId)
            .ToListAsync();

        return turmas.ConvertAll(t => t.ToOut());
    }
}
