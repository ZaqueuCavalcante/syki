namespace Syki.Back.GetCurrentEnrollmentPeriod;

public class GetCurrentEnrollmentPeriodService(SykiDbContext ctx)
{
    public async Task<EnrollmentPeriodOut> Get(Guid institutionId)
    {
        var today = DateOnly.FromDateTime(DateTime.Now);
        var period = await ctx.EnrollmentPeriods.AsNoTracking()
            .Where(p => p.InstitutionId == institutionId && p.Start <= today && p.End >= today)
            .FirstOrDefaultAsync();
        
        if (period == null)
            return new EnrollmentPeriodOut { };
        
        return period.ToOut();
    }
}
