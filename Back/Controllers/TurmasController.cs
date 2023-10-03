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
public class TurmasController : ControllerBase
{
    private readonly SykiDbContext _ctx;
    public TurmasController(SykiDbContext ctx) => _ctx = ctx;

    [HttpPost("")]
    public async Task<IActionResult> Create([FromBody] TurmaIn body)
    {
        var turma = new Turma
        {
            Id = Guid.NewGuid(),
            FaculdadeId = User.Facul(),
            DisciplinaId = body.DisciplinaId,
            ProfessorId = body.ProfessorId,
            Periodo = body.Periodo,
        };
        _ctx.Turmas.Add(turma);

        body.Ofertas.ForEach(x =>
        {
            var vinculo = new OfertaTurma
            {
                OfertaId = x, TurmaId = turma.Id,
            };
            _ctx.Add(vinculo);
        });

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
