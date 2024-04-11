namespace Syki.Back.Features.Academico.CreateDisciplina;

public class CreateDisciplinaService(SykiDbContext ctx)
{
    public async Task<DisciplinaOut> Create(Guid faculdadeId, DisciplinaIn data)
    {
        var disciplina = new Disciplina(
            faculdadeId,
            data.Nome
        );

        var cursos = await ctx.Cursos
            .Where(c => c.FaculdadeId == faculdadeId && data.Cursos.Contains(c.Id))
            .Select(c => c.Id)
            .ToListAsync();

        cursos.ForEach(id => disciplina.Vinculos.Add(new() { CursoId = id }));

        ctx.Add(disciplina);
        await ctx.SaveChangesAsync();

        return disciplina.ToOut();
    }
}
