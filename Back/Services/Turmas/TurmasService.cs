namespace Syki.Back.Services;

public class TurmasService(SykiDbContext ctx) : ITurmasService
{
    public async Task<TurmaOut> Create(Guid faculdadeId, TurmaIn data)
    {
        var disciplinaOk = await ctx.Disciplinas
            .AnyAsync(x => x.FaculdadeId == faculdadeId && x.Id == data.DisciplinaId);
        if (!disciplinaOk)
            Throw.DE004.Now();

        var professorOk = await ctx.Professores
            .AnyAsync(p => p.FaculdadeId == faculdadeId && p.Id == data.ProfessorId);
        if (!professorOk)
            Throw.DE018.Now();

        var periodoOk = await ctx.AcademicPeriods
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

        ctx.Turmas.Add(turma);
        await ctx.SaveChangesAsync();

        turma = await ctx.Turmas.AsNoTracking()
            .Include(t => t.Disciplina)
            .Include(t => t.Professor)
            .Include(t => t.Horarios)
            .FirstAsync(x => x.Id == turma.Id);

        return turma.ToOut();
    }

    public async Task<List<TurmaOut>> GetAll(Guid faculdadeId)
    {
        var turmas = await ctx.Turmas.AsNoTracking()
            .Include(t => t.Disciplina)
            .Include(t => t.Professor)
            .Include(t => t.Horarios)
            .Where(c => c.FaculdadeId == faculdadeId)
            .ToListAsync();

        return turmas.ConvertAll(t => t.ToOut());
    }
}
