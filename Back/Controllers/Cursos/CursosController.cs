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
public class CursosController : ControllerBase
{
    private readonly SykiDbContext _ctx;
    public CursosController(SykiDbContext ctx) => _ctx = ctx;

    [HttpPost("")]
    public async Task<IActionResult> Create([FromBody] CursoIn body)
    {
        var curso = new Curso(body.Nome, body.Tipo, User.Facul());

        await _ctx.Cursos.AddAsync(curso);

        await _ctx.SaveChangesAsync();

        return Ok(curso);
    }

    [HttpGet("")]
    public async Task<IActionResult> GetAll()
    {
        var cursos = await _ctx.Cursos
            .Where(c => c.FaculdadeId == User.Facul()).ToListAsync();

        return Ok(cursos);
    }
}
