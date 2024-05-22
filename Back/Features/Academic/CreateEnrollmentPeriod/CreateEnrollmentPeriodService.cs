namespace Syki.Back.Features.Academic.CreateEnrollmentPeriod;

public class CreateEnrollmentPeriodService(SykiDbContext ctx)
{
    public async Task<EnrollmentPeriodOut> Create(Guid institutionId, CreateEnrollmentPeriodIn data)
    {
        var period = new EnrollmentPeriod(data.Id, institutionId, data.StartAt, data.EndAt);

        ctx.Add(period);
        await ctx.SaveChangesAsync();

        return period.ToOut();
    }
}
