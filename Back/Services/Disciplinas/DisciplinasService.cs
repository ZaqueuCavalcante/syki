namespace Syki.Back.Services;

public class DisciplinasService : IDisciplinasService
{
    private readonly SykiDbContext _ctx;
    public DisciplinasService(SykiDbContext ctx) => _ctx = ctx;

    public async Task<DisciplinaOut> Create(Guid faculdadeId, DisciplinaIn data)
    {
        var disciplina = new Disciplina(
            faculdadeId,
            data.Nome
        );

        var cursos = await _ctx.Cursos
            .Where(c => c.FaculdadeId == faculdadeId && data.Cursos.Contains(c.Id))
            .Select(c => c.Id)
            .ToListAsync();

        cursos.ForEach(id => disciplina.Vinculos.Add(new() { CursoId = id }));

        _ctx.Add(disciplina);
        await _ctx.SaveChangesAsync();

        return disciplina.ToOut();
    }

    public async Task<List<DisciplinaOut>> GetAll(Guid faculdadeId, Guid? cursoId)
    {
        var ids = await _ctx.CursosDisciplinas
            .Where(cd => cd.CursoId == cursoId)
            .Select(cd => cd.DisciplinaId)
            .ToListAsync();

        if (cursoId != null && ids.Count == 0) return [];

        var disciplinas = await _ctx.Disciplinas
            .Where(d => d.FaculdadeId == faculdadeId && (ids.Count == 0 || ids.Contains(d.Id)))
            .OrderBy(d => d.Nome)
            .ToListAsync();

        return disciplinas.ConvertAll(d => d.ToOut());
    }
}
