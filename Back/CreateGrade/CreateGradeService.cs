namespace Syki.Back.CreateGrade;

public class CreateGradeService(SykiDbContext ctx)
{
    public async Task<GradeOut> Create(Guid institutionId, GradeIn data)
    {
        var cursoValido = await ctx.Cursos
            .AnyAsync(c => c.FaculdadeId == institutionId && c.Id == data.CursoId);

        if (!cursoValido)
            Throw.DE002.Now();

        var grade = new Grade(
            institutionId,
            data.CursoId,
            data.Nome
        );

        var disciplinas = await ctx.CursosDisciplinas.AsNoTracking()
            .Where(x => x.CursoId == data.CursoId)
            .Select(x => x.DisciplinaId)
            .ToListAsync();

        if (!data.Disciplinas.ConvertAll(d => d.Id).IsSubsetOf(disciplinas))
            Throw.DE003.Now();

        data.Disciplinas.ForEach(d => grade.Vinculos.Add(
            new GradeDisciplina(d.Id, d.Periodo, d.Creditos, d.CargaHoraria)));

        ctx.Grades.Add(grade);
        await ctx.SaveChangesAsync();

        grade = await ctx.Grades.AsNoTracking()
            .Include(g => g.Curso)
            .Include(x => x.Disciplinas)
            .Include(g => g.Vinculos)
            .FirstAsync(x => x.Id == grade.Id);

        return grade.ToOut();
    }
}
