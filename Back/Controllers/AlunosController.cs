using Syki.Dtos;
using Syki.Back.Domain;
using Syki.Back.Database;
using Syki.Back.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using static Syki.Back.Configs.AuthorizationConfigs;

namespace Syki.Back.Controllers;

[ApiController, Route("[controller]")]
public class AlunosController : ControllerBase
{
    private readonly SykiDbContext _ctx;
    public AlunosController(SykiDbContext ctx) => _ctx = ctx;

    [HttpPost("")]
    [Authorize(Roles = Academico)]
    public async Task<IActionResult> Create([FromBody] AlunoIn body)
    {
        var aluno = new Aluno(User.Facul(), body.Nome);
        aluno.OfertaId = body.OfertaId;

        await _ctx.Alunos.AddAsync(aluno);

        await _ctx.SaveChangesAsync();

        return Ok(aluno);
    }

    [HttpGet("disciplinas")]
    [Authorize(Roles = Configs.AuthorizationConfigs.Aluno)]
    public async Task<IActionResult> GetDisciplinas()
    {
        var aluno = await _ctx.Alunos.FirstAsync(a => a.UserId == User.Id());
        var oferta = await _ctx.Ofertas.FirstAsync(o => o.Id == aluno.OfertaId);
        var grade = await _ctx.Grades
            .Include(g => g.Curso)
            .Include(g => g.Disciplinas)
            .Include(g => g.Vinculos)
            .FirstAsync(g => g.Id == oferta.GradeId);

        return Ok(grade.ToOut().Disciplinas.OrderBy(d => d.Periodo));
    }

    [HttpGet("")]
    [Authorize(Roles = Academico)]
    public async Task<IActionResult> GetAll()
    {
        var alunos = await _ctx.Alunos
            .Include(a => a.Oferta)
                .ThenInclude(o => o.Curso)
            .Where(c => c.FaculdadeId == User.Facul())
            .ToListAsync();
        
        return Ok(alunos.ConvertAll(p => p.ToOut()));
    }
}
