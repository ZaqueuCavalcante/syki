namespace Syki.Back.Features.Academic.CreateEnrollmentPeriod;

public class CreateEnrollmentPeriodService(SykiDbContext ctx, HybridCache cache) : IAcademicService
{
    public async Task<OneOf<EnrollmentPeriodOut, SykiError>> Create(Guid institutionId, CreateEnrollmentPeriodIn data)
    {
        var academicPeriodExists = await ctx.AcademicPeriodExists(institutionId, data.Id);
        if (!academicPeriodExists) return new AcademicPeriodNotFound();

        var enrollmentPeriodExists = await ctx.EnrollmentPeriods.AnyAsync(x => x.InstitutionId == institutionId && x.Id == data.Id);
        if (enrollmentPeriodExists) return new EnrollmentPeriodAlreadyExists();
        
        var result = EnrollmentPeriod.New(data.Id, institutionId, data.StartAt, data.EndAt);

        if (result.IsError()) return result.GetError();

        var period = result.GetSuccess();
        ctx.Add(period);
        await ctx.SaveChangesAsync();

        await cache.RemoveAsync($"enrollmentPeriods:{institutionId}");

        return period.ToOut();
    }

    public async Task CreateWithThrowOnError(Guid institutionId, CreateEnrollmentPeriodIn data)
    {
        (await Create(institutionId, data)).ThrowOnError();
    }
}
