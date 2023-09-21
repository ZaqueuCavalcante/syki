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
public class CampiController : ControllerBase
{
    private readonly SykiDbContext _ctx;
    public CampiController(SykiDbContext ctx) => _ctx = ctx;

    [HttpPost("")]
    public async Task<IActionResult> Create([FromBody] CampusIn body)
    {
        var campus = new Campus(body.Nome, User.Facul());

        await _ctx.Campi.AddAsync(campus);

        await _ctx.SaveChangesAsync();

        return Ok(campus);
    }

    [HttpGet("")]
    public async Task<IActionResult> GetAll()
    {
        var campi = await _ctx.Campi
            .Where(c => c.FaculdadeId == User.Facul()).ToListAsync();

        return Ok(campi);
    }
}
