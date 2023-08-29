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
            body.Creditos,body.CargaHoraria
        );

        await _ctx.Disciplinas.AddAsync(disciplina);

        await _ctx.SaveChangesAsync();

        return Ok(disciplina);
    }

    [HttpGet("")]
    public async Task<IActionResult> GetAll()
    {
        var disciplinas = await _ctx.Disciplinas
            .Where(c => c.FaculdadeId == User.Facul()).ToListAsync();

        return Ok(disciplinas);
    }
}
