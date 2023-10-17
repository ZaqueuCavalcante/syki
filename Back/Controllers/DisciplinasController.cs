using Syki.Dtos;
using Syki.Back.Domain;
using Syki.Back.Database;
using Syki.Back.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using static Syki.Back.Configs.AuthorizationConfigs;

namespace Syki.Back.Controllers;

[Authorize(Roles = Academico)]
[ApiController, Route("[controller]")]
public class DisciplinasController : ControllerBase
{
    private readonly SykiDbContext _ctx;
    public DisciplinasController(SykiDbContext ctx) => _ctx = ctx;

    [HttpPost("")]
    public async Task<IActionResult> Create([FromBody] DisciplinaIn body)
    {
        var disciplina = new Disciplina(
            body.Nome,
            User.Facul(),
            body.CargaHoraria
        );

        foreach (var id in body.Cursos)
        {
            disciplina.Vinculos.Add(new CursoDisciplina { CursoId = id});
        }

        await _ctx.Disciplinas.AddAsync(disciplina);

        await _ctx.SaveChangesAsync();

        return Ok(disciplina);
    }

    [HttpGet("")]
    public async Task<IActionResult> GetAll([FromQuery] Guid? cursoId)
    {
        var ids = await _ctx.CursosDisciplinas
            .Where(cd => cd.CursoId == cursoId)
            .Select(cd => cd.DisciplinaId)
            .ToListAsync();

        if (cursoId != null && ids.Count == 0)
        {
            return Ok(new List<DisciplinaOut>());
        }

        var disciplinas = await _ctx.Disciplinas
            .Where(d => d.FaculdadeId == User.Facul() && (ids.Count == 0 || ids.Contains(d.Id)))
            .OrderBy(d => d.Nome)
            .ToListAsync();

        return Ok(disciplinas.ConvertAll(d => d.ToOut()));
    }
}
