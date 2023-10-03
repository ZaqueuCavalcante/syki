using Syki.Dtos;
using Syki.Back.Domain;
using Syki.Back.Database;
using Syki.Back.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using static Syki.Back.Configs.AuthorizationConfigs;

namespace Syki.Back.Controllers;

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
            Id = Guid.NewGuid(),
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
            .Include(g => g.Curso)
            .Include(x => x.Disciplinas)
            .FirstAsync(x => x.Id == grade.Id);

        return Ok(grade.ToOut());
    }

    [HttpGet("")]
    public async Task<IActionResult> GetAll()
    {
        var grades = await _ctx.Grades
            .Where(c => c.FaculdadeId == User.Facul())
            .Include(g => g.Curso)
            .Include(g => g.Disciplinas)
            .Include(g => g.Vinculos)
            .ToListAsync();

        return Ok(grades.ConvertAll(g => g.ToOut()));
    }

    [HttpGet("{id}/disciplinas")]
    public async Task<IActionResult> GetDisciplinas([FromRoute] Guid id)
    {
        var grade = await _ctx.Grades
            .Where(c => c.FaculdadeId == User.Facul() && c.Id == id)
            .Include(g => g.Disciplinas)
            .FirstOrDefaultAsync();
        
        if (grade == null)
            return Ok(new List<DisciplinaOut>());

        return Ok(grade.Disciplinas.ConvertAll(g => g.ToOut()));
    }
}
