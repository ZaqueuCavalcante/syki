using Syki.Shared;
using Syki.Back.Domain;
using Syki.Back.Database;
using Microsoft.EntityFrameworkCore;

namespace Syki.Back.Services;

public class CursosService : ICursosService
{
    private readonly SykiDbContext _ctx;
    public CursosService(SykiDbContext ctx) => _ctx = ctx;

    public async Task<CursoOut> Create(Guid faculdadeId, CursoIn data)
    {
        var curso = new Curso(faculdadeId, data.Nome, data.Tipo);

        await _ctx.Cursos.AddAsync(curso);

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
}
