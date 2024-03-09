using Syki.Back.Domain;

namespace Syki.Back.Services;

public class CursosService : ICursosService
{
    private readonly SykiDbContext _ctx;
    public CursosService(SykiDbContext ctx) => _ctx = ctx;

    public async Task<CursoOut> Create(Guid faculdadeId, CursoIn data)
    {
        var curso = new Curso(faculdadeId, data.Nome, data.Tipo);

        _ctx.Add(curso);
        await _ctx.SaveChangesAsync();

        return curso.ToOut();
    }

    public async Task<List<CursoOut>> GetAll(Guid faculdadeId)
    {
        var cursos = await _ctx.Cursos
            .Where(c => c.FaculdadeId == faculdadeId)
            .OrderBy(c => c.Nome)
            .ToListAsync();

        return cursos.ConvertAll(c => c.ToOut());
    }

    public async Task<List<CursoOut>> GetAllWithDisciplinas(Guid faculdadeId)
    {
        var cursos = await _ctx.Cursos
            .Where(c => c.FaculdadeId == faculdadeId && c.Disciplinas.Count > 0)
            .OrderBy(c => c.Nome)
            .ToListAsync();

        return cursos.ConvertAll(c => c.ToOut());
    }

    public async Task<List<CursoDisciplinaOut>> GetDisciplinas(Guid id, Guid faculdadeId)
    {
        var ids = await _ctx.CursosDisciplinas
            .Where(x => x.CursoId == id).Select(x => x.DisciplinaId).ToListAsync();

        var disciplinas = await _ctx.Disciplinas
            .Where(d => ids.Contains(d.Id))
            .Select(d => new CursoDisciplinaOut { Id = d.Id, Nome = d.Nome })
            .OrderBy(d => d.Nome)
            .ToListAsync();

        return disciplinas;
    }
}
