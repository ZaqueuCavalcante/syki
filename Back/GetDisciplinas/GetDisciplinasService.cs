namespace Syki.Back.GetDisciplinas;

public class GetDisciplinasService(SykiDbContext ctx)
{
    public async Task<List<DisciplinaOut>> Get(Guid faculdadeId, Guid? cursoId)
    {
        var ids = await ctx.CursosDisciplinas
            .Where(cd => cd.CursoId == cursoId)
            .Select(cd => cd.DisciplinaId)
            .ToListAsync();

        if (cursoId != null && ids.Count == 0) return [];

        var disciplinas = await ctx.Disciplinas
            .Where(d => d.FaculdadeId == faculdadeId && (ids.Count == 0 || ids.Contains(d.Id)))
            .OrderBy(d => d.Nome)
            .ToListAsync();

        return disciplinas.ConvertAll(d => d.ToOut());
    }
}
