namespace Syki.Back.GetCursoDisciplinas;

public class GetCursoDisciplinasService(SykiDbContext ctx)
{
    public async Task<List<CursoDisciplinaOut>> Get(Guid id, Guid institutionId)
    {
        var ids = await ctx.CursosDisciplinas
            .Where(x => x.CursoId == id).Select(x => x.DisciplinaId).ToListAsync();

        var disciplinas = await ctx.Disciplinas
            .Where(d => d.FaculdadeId == institutionId && ids.Contains(d.Id))
            .Select(d => new CursoDisciplinaOut { Id = d.Id, Nome = d.Nome })
            .OrderBy(d => d.Nome)
            .ToListAsync();

        return disciplinas;
    }
}
