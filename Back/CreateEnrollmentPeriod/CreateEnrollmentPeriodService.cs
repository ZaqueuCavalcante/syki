namespace Syki.Back.CreateEnrollmentPeriod;

public class CreateEnrollmentPeriodService(SykiDbContext ctx)
{
    public async Task<EnrollmentPeriodOut> Create(Guid institutionId, CreateEnrollmentPeriodIn data)
    {
        var period = new EnrollmentPeriod(data.Id, institutionId, data.Start, data.End);

        ctx.Add(period);
        await ctx.SaveChangesAsync();

        return period.ToOut();
    }
}
