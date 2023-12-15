using Syki.Shared;
using Syki.Back.Domain;
using Syki.Back.Database;
using Microsoft.EntityFrameworkCore;

namespace Syki.Back.Services;

public class OfertasService : IOfertasService
{
    private readonly SykiDbContext _ctx;
    public OfertasService(SykiDbContext ctx) => _ctx = ctx;

    public async Task<OfertaOut> Create(Guid faculdadeId, OfertaIn data)
    {
        var oferta = new Oferta(
            faculdadeId,
            data.CampusId,
            data.CursoId,
            data.GradeId,
            data.Periodo,
            data.Turno
        );

        _ctx.Ofertas.Add(oferta);
        await _ctx.SaveChangesAsync();

        oferta = await _ctx.Ofertas
            .Include(x => x.Campus)
            .Include(x => x.Curso)
            .Include(x => x.Grade)
            .FirstAsync(x => x.Id == oferta.Id);

        return oferta.ToOut();
    }

    public async Task<List<OfertaOut>> GetAll(Guid faculdadeId, Guid? disciplinaId)
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
                .Where(c => c.FaculdadeId == faculdadeId && grades.Contains(c.GradeId))
                .ToListAsync();
        }
        else
        {
            ofertas = await _ctx.Ofertas
                .Include(x => x.Campus)
                .Include(x => x.Curso)
                .Include(x => x.Grade)
                .Where(c => c.FaculdadeId == faculdadeId)
                .ToListAsync();
        }

        return ofertas.ConvertAll(o => o.ToOut());
    }
}
