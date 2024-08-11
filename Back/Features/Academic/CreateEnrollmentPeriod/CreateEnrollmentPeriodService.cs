namespace Syki.Back.Features.Academic.CreateEnrollmentPeriod;

public class CreateEnrollmentPeriodService(SykiDbContext ctx) : IAcademicService
{
    public async Task<OneOf<EnrollmentPeriodOut, SykiError>> Create(Guid institutionId, CreateEnrollmentPeriodIn data)
    {
        var periodExists = await ctx.AcademicPeriodExists(institutionId, data.Id);
        if (!periodExists) return new AcademicPeriodNotFound();

        var result = EnrollmentPeriod.New(data.Id, institutionId, data.StartAt, data.EndAt);

        if (result.IsError()) return result.GetError();

        var period = result.GetSuccess();
        ctx.Add(period);
        await ctx.SaveChangesAsync();

        return period.ToOut();
    }
}
