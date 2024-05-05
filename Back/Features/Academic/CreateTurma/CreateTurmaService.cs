namespace Syki.Back.CreateTurma;

public class CreateTurmaService(SykiDbContext ctx)
{
    public async Task<TurmaOut> Create(Guid institutionId, TurmaIn data)
    {
        var disciplinaOk = await ctx.Disciplinas
            .AnyAsync(x => x.InstitutionId == institutionId && x.Id == data.DisciplinaId);
        if (!disciplinaOk)
            Throw.DE004.Now();

        var professorOk = await ctx.Professores
            .AnyAsync(p => p.InstitutionId == institutionId && p.Id == data.ProfessorId);
        if (!professorOk)
            Throw.DE018.Now();

        var periodoOk = await ctx.AcademicPeriods
            .AnyAsync(p => p.InstitutionId == institutionId && p.Id == data.Periodo);
        if (!periodoOk)
            Throw.DE005.Now();

        var horarios = data.Horarios.ConvertAll(h => new Horario(h.Dia, h.Start, h.End));
        var turma = new Turma(
            institutionId,
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
}
