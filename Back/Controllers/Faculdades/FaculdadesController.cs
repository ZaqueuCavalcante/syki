using Syki.Domain;
using Syki.Database;
using Microsoft.AspNetCore.Mvc;

namespace Syki.Controllers;

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
}
