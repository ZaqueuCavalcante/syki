namespace Syki.Back.GetEnrollmentPeriods;

public class GetEnrollmentPeriodsService(SykiDbContext ctx)
{
    public async Task<List<AcademicPeriodOut>> Get(Guid institutionId)
    {
        var periods = await ctx.AcademicPeriods.AsNoTracking()
            .Where(c => c.InstitutionId == institutionId)
            .ToListAsync();
        
        return periods.ConvertAll(p => p.ToOut());
    }
}
