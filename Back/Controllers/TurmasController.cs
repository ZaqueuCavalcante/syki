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
public class TurmasController : ControllerBase
{
    private readonly SykiDbContext _ctx;
    public TurmasController(SykiDbContext ctx) => _ctx = ctx;

    [HttpPost("")]
    public async Task<IActionResult> Create([FromBody] TurmaIn body)
    {
        var turma = new Turma
        {
            FaculdadeId = User.Facul(),
            DisciplinaId = body.DisciplinaId,
            ProfessorId = body.ProfessorId,
            Periodo = body.Periodo,
        };

        _ctx.Turmas.Add(turma);

        await _ctx.SaveChangesAsync();

        turma = await _ctx.Turmas
            .Include(t => t.Disciplina)
            .Include(t => t.Professor)
            .FirstAsync(x => x.Id == turma.Id);

        return Ok(turma.ToOut());
    }

    [HttpGet("")]
    public async Task<IActionResult> GetAll()
    {
        var turmas = await _ctx.Turmas
            .Include(t => t.Disciplina)
            .Include(t => t.Professor)
            .Where(c => c.FaculdadeId == User.Facul())
            .ToListAsync();

        return Ok(turmas.ConvertAll(t => t.ToOut()));
    }
}
