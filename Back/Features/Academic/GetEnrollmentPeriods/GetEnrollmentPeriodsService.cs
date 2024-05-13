namespace Syki.Back.Features.Academic.GetEnrollmentPeriods;

public class GetEnrollmentPeriodsService(SykiDbContext ctx)
{
    public async Task<List<EnrollmentPeriodOut>> Get(Guid institutionId)
    {
        var periods = await ctx.EnrollmentPeriods.AsNoTracking()
            .Where(c => c.InstitutionId == institutionId)
            .ToListAsync();
        
        return periods.ConvertAll(p => p.ToOut());
    }
}
