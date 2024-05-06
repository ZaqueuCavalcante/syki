namespace Syki.Back.GetCursoDisciplinas;

public class GetCursoDisciplinasService(SykiDbContext ctx)
{
    public async Task<List<CourseDisciplineOut>> Get(Guid id, Guid institutionId)
    {
        var ids = await ctx.CursosDisciplinas
            .Where(x => x.CursoId == id).Select(x => x.DisciplinaId).ToListAsync();

        var disciplinas = await ctx.Disciplinas
            .Where(d => d.InstitutionId == institutionId && ids.Contains(d.Id))
            .Select(d => new CourseDisciplineOut { Id = d.Id, Name = d.Name })
            .OrderBy(d => d.Name)
            .ToListAsync();

        return disciplinas;
    }
}
