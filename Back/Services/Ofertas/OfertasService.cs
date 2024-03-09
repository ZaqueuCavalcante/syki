namespace Syki.Back.Services;

public class OfertasService(SykiDbContext ctx) : IOfertasService
{
    public async Task<OfertaOut> Create(Guid faculdadeId, OfertaIn data)
    {
        var campusOk = await ctx.Campi
            .AnyAsync(c => c.InstitutionId == faculdadeId && c.Id == data.CampusId);
        if (!campusOk)
            Throw.DE010.Now();

        var periodoOk = await ctx.AcademicPeriods
            .AnyAsync(p => p.InstitutionId == faculdadeId && p.Id == data.Periodo);
        if (!periodoOk)
            Throw.DE005.Now();

        var cursoOk = await ctx.Cursos
            .AnyAsync(c => c.FaculdadeId == faculdadeId && c.Id == data.CursoId);
        if (!cursoOk)
            Throw.DE002.Now();

        var gradeOk = await ctx.Grades
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

        ctx.Ofertas.Add(oferta);
        await ctx.SaveChangesAsync();

        oferta = await ctx.Ofertas.AsNoTracking()
            .Include(x => x.Campus)
            .Include(x => x.Curso)
            .Include(x => x.Grade)
            .FirstAsync(x => x.Id == oferta.Id);

        return oferta.ToOut();
    }

    public async Task<List<OfertaOut>> GetAll(Guid faculdadeId)
    {
        var ofertas = await ctx.Ofertas.AsNoTracking()
            .Include(x => x.Campus)
            .Include(x => x.Curso)
            .Include(x => x.Grade)
            .Where(c => c.FaculdadeId == faculdadeId)
            .ToListAsync();

        return ofertas.ConvertAll(o => o.ToOut());
    }
}
