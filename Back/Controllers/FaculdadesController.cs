using Syki.Shared;
using Syki.Back.Domain;
using Syki.Back.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using static Syki.Back.Configs.AuthorizationConfigs;

namespace Syki.Back.Controllers;

[Authorize(Roles = Adm)]
[ApiController, Route("[controller]")]
public class FaculdadesController : ControllerBase
{
    private readonly SykiDbContext _ctx;
    public FaculdadesController(SykiDbContext ctx) => _ctx = ctx;

    [HttpPost("")]
    public async Task<IActionResult> Create([FromBody] FaculdadeIn body)
    {
        var faculdade = new Faculdade(body.Nome);

        await _ctx.Faculdades.AddAsync(faculdade);

        await _ctx.SaveChangesAsync();

        return Ok(faculdade);
    }

    [HttpGet("")]
    public async Task<IActionResult> GetAll()
    {
        var faculdades = await _ctx.Faculdades
            .Where(x => x.Id != Guid.Empty)
            .ToListAsync();

        return Ok(faculdades);
    }
}
