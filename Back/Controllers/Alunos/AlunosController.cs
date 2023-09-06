using Syki.Dtos;
using Syki.Domain;
using Syki.Database;
using Microsoft.AspNetCore.Mvc;

namespace Syki.Controllers;

[ApiController, Route("[controller]")]
public class AlunosController : ControllerBase
{
    private readonly SykiDbContext _ctx;

    public AlunosController(SykiDbContext ctx) => _ctx = ctx;

    [HttpPost("")]
    public async Task<IActionResult> Create([FromBody] AlunoIn body)
    {
        var aluno = new Aluno(body.Nome);

        await _ctx.Alunos.AddAsync(aluno);

        await _ctx.SaveChangesAsync();

        return Ok(aluno);
    }
}
