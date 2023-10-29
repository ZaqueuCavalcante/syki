using Syki.Shared;
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
public class ProfessoresController : ControllerBase
{
    private readonly SykiDbContext _ctx;
    public ProfessoresController(SykiDbContext ctx) => _ctx = ctx;

    [HttpPost("")]
    public async Task<IActionResult> Create([FromBody] ProfessorIn body)
    {
        var professor = new Professor(User.Facul(), body.Nome);

        await _ctx.Professores.AddAsync(professor);

        await _ctx.SaveChangesAsync();

        return Ok(professor.ToOut());
    }

    [HttpGet("")]
    public async Task<IActionResult> GetAll()
    {
        var professores = await _ctx.Professores
            .Where(c => c.FaculdadeId == User.Facul())
            .ToListAsync();

        return Ok(professores.ConvertAll(p => p.ToOut()));
    }
}
