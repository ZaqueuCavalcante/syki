using Syki.Dtos;
using Syki.Domain;
using Syki.Database;
using Syki.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using static Syki.Configs.AuthorizationConfigs;

namespace Syki.Controllers;

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

        await _ctx.Disciplinas.AddAsync(disciplina);

        await _ctx.SaveChangesAsync();

        return Ok(disciplina);
    }

    [HttpGet("")]
    public async Task<IActionResult> GetAll([FromQuery] long? cursoId)
    {
        var ids = await _ctx.CursosDisciplinas
            .Where(cd => cd.CursoId == cursoId)
            .Select(cd => cd.DisciplinaId)
            .ToListAsync();

        var disciplinas = await _ctx.Disciplinas
            .Where(d => d.FaculdadeId == User.Facul() && (ids.Count == 0 || ids.Contains(d.Id)))
            .ToListAsync();

        return Ok(disciplinas.ConvertAll(d => d.ToOut()));
    }
}
