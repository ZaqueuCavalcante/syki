namespace Syki.Back.Services;

public class GradesService(SykiDbContext ctx) : IGradesService
{
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
