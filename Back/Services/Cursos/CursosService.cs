namespace Syki.Back.Services;

public class CursosService(SykiDbContext ctx) : ICursosService
{
    public async Task<List<CursoOut>> GetAllWithDisciplinas(Guid faculdadeId)
    {
        var cursos = await ctx.Cursos
            .Where(c => c.FaculdadeId == faculdadeId && c.Disciplinas.Count > 0)
            .OrderBy(c => c.Nome)
            .ToListAsync();

        return cursos.ConvertAll(c => c.ToOut());
    }

    public async Task<List<CursoDisciplinaOut>> GetDisciplinas(Guid id, Guid faculdadeId)
    {
        var ids = await ctx.CursosDisciplinas
            .Where(x => x.CursoId == id).Select(x => x.DisciplinaId).ToListAsync();

        var disciplinas = await ctx.Disciplinas
            .Where(d => ids.Contains(d.Id))
            .Select(d => new CursoDisciplinaOut { Id = d.Id, Nome = d.Nome })
            .OrderBy(d => d.Nome)
            .ToListAsync();

        return disciplinas;
    }
}
