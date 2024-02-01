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
        var ofertas = await _ctx.Ofertas
            .Where(o => o.FaculdadeId == faculdadeId && o.Id == data.OfertaId).ToListAsync();
        if (ofertas.Count == 0)
            Throw.DE0009.Now();

        var gradeId = await _ctx.Ofertas
            .Where(o => o.Id == data.OfertaId).Select(o => o.GradeId).FirstAsync();
        var disciplinaOk = await _ctx.GradesDisciplinas
            .AnyAsync(x => x.GradeId == gradeId && x.DisciplinaId == data.DisciplinaId);
        if (!disciplinaOk)
            Throw.DE0002.Now();

        var professorOk = await _ctx.Professores
            .AnyAsync(p => p.FaculdadeId == faculdadeId && p.Id == data.ProfessorId);
        if (!professorOk)
            Throw.DE0015.Now();

        var periodoOk = await _ctx.Periodos
            .AnyAsync(p => p.FaculdadeId == faculdadeId && p.Id == data.Periodo);
        if (!periodoOk)
            Throw.DE0003.Now();

        var horarios = new List<Horario>() { new(data.Dia, data.Start, data.End) };

        var turma = new Turma(
            faculdadeId,
            data.DisciplinaId,
            data.ProfessorId,
            data.Periodo,
            ofertas,
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
