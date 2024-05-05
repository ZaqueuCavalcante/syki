namespace Syki.Back.Features.Academico.CreateDisciplina;

public class CreateDisciplinaService(SykiDbContext ctx)
{
    public async Task<DisciplinaOut> Create(Guid institutionId, DisciplinaIn data)
    {
        var disciplina = new Disciplina(
            institutionId,
            data.Name
        );

        var cursos = await ctx.Cursos
            .Where(c => c.InstitutionId == institutionId && data.Cursos.Contains(c.Id))
            .Select(c => c.Id)
            .ToListAsync();

        cursos.ForEach(id => disciplina.Vinculos.Add(new() { CursoId = id }));

        ctx.Add(disciplina);
        await ctx.SaveChangesAsync();

        return disciplina.ToOut();
    }
}
