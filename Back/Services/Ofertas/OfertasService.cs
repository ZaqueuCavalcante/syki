using Syki.Back.Domain;

namespace Syki.Back.Services;

public class OfertasService : IOfertasService
{
    private readonly SykiDbContext _ctx;
    public OfertasService(SykiDbContext ctx) => _ctx = ctx;

    public async Task<OfertaOut> Create(Guid faculdadeId, OfertaIn data)
    {
        var campusOk = await _ctx.Campi
            .AnyAsync(c => c.InstitutionId == faculdadeId && c.Id == data.CampusId);
        if (!campusOk)
            Throw.DE010.Now();

        var periodoOk = await _ctx.AcademicPeriods
            .AnyAsync(p => p.InstitutionId == faculdadeId && p.Id == data.Periodo);
        if (!periodoOk)
            Throw.DE005.Now();

        var cursoOk = await _ctx.Cursos
            .AnyAsync(c => c.FaculdadeId == faculdadeId && c.Id == data.CursoId);
        if (!cursoOk)
            Throw.DE002.Now();

        var gradeOk = await _ctx.Grades
            .AnyAsync(g => g.FaculdadeId == faculdadeId && g.Id == data.GradeId && g.CursoId == data.CursoId);
        if (!gradeOk)
            Throw.DE011.Now();

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
