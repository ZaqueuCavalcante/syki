namespace Syki.Back.Features.Academic.CreateEnrollmentPeriod;

public class CreateEnrollmentPeriodService(SykiDbContext ctx)
{
    public async Task<OneOf<EnrollmentPeriodOut, SykiError>> Create(Guid institutionId, CreateEnrollmentPeriodIn data)
    {
        var result = EnrollmentPeriod.New(data.Id, institutionId, data.StartAt, data.EndAt);

        return await result.Match<Task<OneOf<EnrollmentPeriodOut, SykiError>>>(
            async period =>
            {
                ctx.Add(period);
                await ctx.SaveChangesAsync();
                return period.ToOut();
            },
            error => Task.FromResult<OneOf<EnrollmentPeriodOut, SykiError>>(error)
        );
    }
}
