using Syki.Dtos;
using Syki.Domain;
using Syki.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using static Syki.Configs.AuthorizationConfigs;

namespace Syki.Controllers;

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
        var faculdades = await _ctx.Faculdades.ToListAsync();

        return Ok(faculdades);
    }
}
