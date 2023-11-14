using Syki.Shared;
using Syki.Back.Domain;
using Syki.Back.Database;
using Syki.Back.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using static Syki.Back.Configs.AuthorizationConfigs;

namespace Syki.Back.Controllers;

[ApiController, Route("[controller]")]
public class LivrosController : ControllerBase
{
    private readonly SykiDbContext _ctx;
    public LivrosController(SykiDbContext ctx) => _ctx = ctx;

    [HttpPost("")]
    [Authorize(Roles = Academico)]
    public async Task<IActionResult> Create([FromBody] LivroIn body)
    {
        var livro = new Livro(User.Facul(), body.Titulo);

        await _ctx.Livros.AddAsync(livro);
        await _ctx.SaveChangesAsync();

        return Ok(livro.ToOut());
    }

    [HttpGet("")]
    [Authorize(Roles = Academico)]
    public async Task<IActionResult> GetAll()
    {
        var livros = await _ctx.Livros
            .Where(c => c.FaculdadeId == User.Facul())
            .ToListAsync();
        
        return Ok(livros.ConvertAll(p => p.ToOut()));
    }
}
