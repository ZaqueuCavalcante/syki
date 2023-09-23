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
public class AlunosController : ControllerBase
{
    private readonly SykiDbContext _ctx;
    public AlunosController(SykiDbContext ctx) => _ctx = ctx;

    [HttpPost("")]
    public async Task<IActionResult> Create([FromBody] AlunoIn body)
    {
        var aluno = new Aluno(User.Facul(), body.Nome);

        await _ctx.Alunos.AddAsync(aluno);

        await _ctx.SaveChangesAsync();

        return Ok(aluno);
    }

    [HttpGet("")]
    public async Task<IActionResult> GetAll()
    {
        var alunos = await _ctx.Alunos
            .Where(c => c.FaculdadeId == User.Facul())
            .ToListAsync();

        return Ok(alunos.ConvertAll(p => p.ToOut()));
    }
}
