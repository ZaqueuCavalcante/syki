namespace Syki.Back.Services;

public class GradesService(SykiDbContext ctx) : IGradesService
{
    public async Task<GradeOut> Create(Guid faculdadeId, GradeIn data)
    {
        var cursoValido = await ctx.Cursos
            .AnyAsync(c => c.FaculdadeId == faculdadeId && c.Id == data.CursoId);

        if (!cursoValido)
            Throw.DE002.Now();

        var grade = new Grade(
            faculdadeId,
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

    public async Task<List<DisciplinaOut>> GetDisciplinas(Guid faculdadeId, Guid id)
    {
        var grade = await ctx.Grades.AsNoTracking()
            .Where(g => g.FaculdadeId == faculdadeId && g.Id == id)
            .Include(g => g.Disciplinas)
            .FirstOrDefaultAsync();

        return grade== null ? [] : grade.Disciplinas.ConvertAll(d => d.ToOut()).OrderBy(d => d.Nome).ToList();
    }

    public async Task<List<GradeOut>> GetAll(Guid faculdadeId)
    {
        var grades = await ctx.Grades.AsNoTracking()
            .Where(c => c.FaculdadeId == faculdadeId)
            .Include(g => g.Curso)
            .Include(g => g.Disciplinas)
            .Include(g => g.Vinculos)
            .OrderBy(g => g.Nome)
            .ToListAsync();

        return grades.ConvertAll(g => g.ToOut());
    }
}
