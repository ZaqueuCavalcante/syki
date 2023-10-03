using Syki.Dtos;
using Syki.Domain;
using Syki.Database;
using Syki.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using static Syki.Configs.AuthorizationConfigs;
using Microsoft.IdentityModel.Protocols;

namespace Syki.Controllers;

[Authorize(Roles = Academico)]
[ApiController, Route("[controller]")]
public class OfertasController : ControllerBase
{
    private readonly SykiDbContext _ctx;
    public OfertasController(SykiDbContext ctx) => _ctx = ctx;

    [HttpPost("")]
    public async Task<IActionResult> Create([FromBody] OfertaIn body)
    {
        var oferta = new Oferta
        {
            Id = Guid.NewGuid(),
            FaculdadeId = User.Facul(),
            CampusId = body.CampusId,
            CursoId = body.CursoId,
            GradeId = body.GradeId,
            Periodo = body.Periodo,
            Turno = body.Turno,
        };

        _ctx.Ofertas.Add(oferta);

        await _ctx.SaveChangesAsync();

        oferta = await _ctx.Ofertas
            .Include(x => x.Campus)
            .Include(x => x.Curso)
            .Include(x => x.Grade)
            .FirstAsync(x => x.Id == oferta.Id);

        return Ok(oferta.ToOut());
    }

    [HttpGet("")]
    public async Task<IActionResult> GetAll([FromQuery] Guid? disciplinaId)
    {
        var ofertas = new List<Oferta>();

        if (disciplinaId != null)
        {
            var grades = await _ctx.GradesDisciplinas
                .Where(gd => gd.DisciplinaId == disciplinaId)
                .Select(gd => gd.GradeId)
                .ToListAsync();

            ofertas = await _ctx.Ofertas
                .Include(x => x.Campus)
                .Include(x => x.Curso)
                .Include(x => x.Grade)
                .Where(c => c.FaculdadeId == User.Facul() && grades.Contains(c.GradeId))
                .ToListAsync();
        }
        else
        {
            ofertas = await _ctx.Ofertas
                .Include(x => x.Campus)
                .Include(x => x.Curso)
                .Include(x => x.Grade)
                .Where(c => c.FaculdadeId == User.Facul())
                .ToListAsync();
        }

        return Ok(ofertas.ConvertAll(o => o.ToOut()));
    }
}
