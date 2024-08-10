namespace Syki.Back.Features.Student.GetCurrentEnrollmentPeriod;

public class GetCurrentEnrollmentPeriodService(SykiDbContext ctx) : IStudentService
{
    public async Task<EnrollmentPeriodOut> Get(Guid institutionId)
    {
        var today = DateOnly.FromDateTime(DateTime.Now);
        var period = await ctx.EnrollmentPeriods.AsNoTracking()
            .Where(p => p.InstitutionId == institutionId && p.StartAt <= today && p.EndAt >= today)
            .FirstOrDefaultAsync();
        
        if (period == null)
            return new EnrollmentPeriodOut { };
        
        return period.ToOut();
    }
}
