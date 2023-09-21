using Syki.Database;
using Syki.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using static Syki.Configs.AuthorizationConfigs;

namespace Syki.Controllers;

[Authorize(Roles = Academico)]
[ApiController, Route("[controller]")]
public class ProfessoresController : ControllerBase
{
    private readonly SykiDbContext _ctx;
    public ProfessoresController(SykiDbContext ctx) => _ctx = ctx;

    [HttpGet("")]
    public async Task<IActionResult> GetAll()
    {
        var professores = await _ctx.Professores
            .Where(c => c.FaculdadeId == User.Facul())
            .ToListAsync();

        return Ok(professores.ConvertAll(p => p.ToOut()));
    }
}
