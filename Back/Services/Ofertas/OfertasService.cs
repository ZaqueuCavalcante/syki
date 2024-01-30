using Syki.Shared;
using Syki.Back.Domain;
using Syki.Back.Database;
using Syki.Back.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Syki.Back.Services;

public class OfertasService : IOfertasService
{
    private readonly SykiDbContext _ctx;
    public OfertasService(SykiDbContext ctx) => _ctx = ctx;

    public async Task<OfertaOut> Create(Guid faculdadeId, OfertaIn data)
    {
        var campusOk = await _ctx.Campi
            .AnyAsync(c => c.FaculdadeId == faculdadeId && c.Id == data.CampusId);
        if (!campusOk)
            Throw.DE0007.Now();

        var periodoOk = await _ctx.Periodos
            .AnyAsync(p => p.FaculdadeId == faculdadeId && p.Id == data.Periodo);
        if (!periodoOk)
            Throw.DE0003.Now();

        var cursoOk = await _ctx.Cursos
            .AnyAsync(c => c.FaculdadeId == faculdadeId && c.Id == data.CursoId);
        if (!cursoOk)
            Throw.DE0001.Now();

        var gradeOk = await _ctx.Grades
            .AnyAsync(g => g.FaculdadeId == faculdadeId && g.Id == data.GradeId && g.CursoId == data.CursoId);
        if (!gradeOk)
            Throw.DE0008.Now();

        var oferta = new Oferta(
            faculdadeId,
            data.CampusId,
            data.CursoId,
            data.GradeId,
            data.Periodo!,
            data.Turno
        );

        _ctx.Ofertas.Add(oferta);
        await _ctx.SaveChangesAsync();

        oferta = await _ctx.Ofertas.AsNoTracking()
            .Include(x => x.Campus)
            .Include(x => x.Curso)
            .Include(x => x.Grade)
            .FirstAsync(x => x.Id == oferta.Id);

        return oferta.ToOut();
    }

    public async Task<List<OfertaOut>> GetAll(Guid faculdadeId)
    {
        var ofertas = await _ctx.Ofertas.AsNoTracking()
            .Include(x => x.Campus)
            .Include(x => x.Curso)
            .Include(x => x.Grade)
            .Where(c => c.FaculdadeId == faculdadeId)
            .ToListAsync();

        return ofertas.ConvertAll(o => o.ToOut());
    }
}
