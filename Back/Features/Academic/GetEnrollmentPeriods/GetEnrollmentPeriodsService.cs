namespace Syki.Back.Features.Academic.GetEnrollmentPeriods;

public class GetEnrollmentPeriodsService(SykiDbContext ctx) : IAcademicService
{
    public async Task<List<EnrollmentPeriodOut>> Get(Guid institutionId)
    {
        var periods = await ctx.EnrollmentPeriods.AsNoTracking()
            .Where(c => c.InstitutionId == institutionId)
            .OrderBy(p => p.Id)
            .ToListAsync();
        
        return periods.ConvertAll(p => p.ToOut());
    }
}
