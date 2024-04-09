namespace Syki.Back.CreateOferta;

public class CreateOfertaService(SykiDbContext ctx)
{
    public async Task<OfertaOut> Create(Guid institutionId, OfertaIn data)
    {
        var campusOk = await ctx.Campi
            .AnyAsync(c => c.InstitutionId == institutionId && c.Id == data.CampusId);
        if (!campusOk)
            Throw.DE010.Now();

        var periodoOk = await ctx.AcademicPeriods
            .AnyAsync(p => p.InstitutionId == institutionId && p.Id == data.Periodo);
        if (!periodoOk)
            Throw.DE005.Now();

        var cursoOk = await ctx.Cursos
            .AnyAsync(c => c.FaculdadeId == institutionId && c.Id == data.CursoId);
        if (!cursoOk)
            Throw.DE002.Now();

        var gradeOk = await ctx.Grades
            .AnyAsync(g => g.FaculdadeId == institutionId && g.Id == data.GradeId && g.CursoId == data.CursoId);
        if (!gradeOk)
            Throw.DE011.Now();

        var oferta = new Oferta(
            institutionId,
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
}
