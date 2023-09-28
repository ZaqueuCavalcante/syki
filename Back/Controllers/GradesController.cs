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
public class GradesController : ControllerBase
{
    private readonly SykiDbContext _ctx;
    public GradesController(SykiDbContext ctx) => _ctx = ctx;

    [HttpPost("")]
    public async Task<IActionResult> Create([FromBody] GradeIn body)
    {
        var grade = new Grade
        {
            FaculdadeId = User.Facul(),
            CursoId = body.CursoId,
            Nome = body.Nome,
            Vinculos = new(),
        };

        foreach (var d in body.Disciplinas)
        {
            grade.Vinculos.Add(new GradeDisciplina {
                DisciplinaId = d.Id,
                Periodo = d.Periodo,
                Creditos = d.Creditos,
                CargaHoraria = d.CargaHoraria,
            });
        }

        _ctx.Grades.Add(grade);
        await _ctx.SaveChangesAsync();

        grade = await _ctx.Grades.AsNoTracking()
            .Include(x => x.Disciplinas)
            .FirstAsync(x => x.Id == grade.Id);

        return Ok(grade.ToOut());
    }

    [HttpGet("")]
    public async Task<IActionResult> GetAll()
    {
        var grades = await _ctx.Grades
            .Where(c => c.FaculdadeId == User.Facul())
            .Include(g => g.Disciplinas)
            .Include(g => g.Vinculos)
            .ToListAsync();

        return Ok(grades.ConvertAll(g => g.ToOut()));
    }
}
