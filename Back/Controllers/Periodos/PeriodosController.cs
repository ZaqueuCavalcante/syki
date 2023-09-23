using Syki.Database;
using Syki.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using static Syki.Configs.AuthorizationConfigs;

namespace Syki.Controllers;

[Authorize(Roles = Academico)]
[ApiController, Route("[controller]")]
public class PeriodosController : ControllerBase
{
    private readonly SykiDbContext _ctx;
    public PeriodosController(SykiDbContext ctx) => _ctx = ctx;

    [HttpGet("")]
    public async Task<IActionResult> GetAll()
    {
        var periodos = await _ctx.Periodos
            .Where(c => c.FaculdadeId == User.Facul())
            .Select(p => p.Id)
            .ToListAsync();

        return Ok(periodos);
    }
}
