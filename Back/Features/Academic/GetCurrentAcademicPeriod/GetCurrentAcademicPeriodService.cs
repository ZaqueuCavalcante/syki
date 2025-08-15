namespace Syki.Back.Features.Academic.GetCurrentAcademicPeriod;

public class GetCurrentAcademicPeriodService(SykiDbContext ctx) : IAcademicService
{
    public async Task<OneOf<AcademicPeriodOut, SykiError>> Get(Guid institutionId)
    {
        var period = await ctx.GetCurrentAcademicPeriod(institutionId);

        if (period == null) return new CurrentAcademicPeriodNotFound();

        return period.ToOut();
    }
}
