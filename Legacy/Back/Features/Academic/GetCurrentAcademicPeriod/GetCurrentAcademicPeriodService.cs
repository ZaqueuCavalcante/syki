namespace Estud.Back.Features.Academic.GetCurrentAcademicPeriod;

public class GetCurrentAcademicPeriodService(EstudDbContext ctx) : IEstudService
{
    public async Task<OneOf<AcademicPeriodOut, EstudError>> Get(Guid institutionId)
    {
        var period = await ctx.GetCurrentAcademicPeriod(institutionId);

        if (period == null) return new CurrentAcademicPeriodNotFound();

        return period.ToOut();
    }
}
