namespace Syki.Back.Features.Academic.UpdateEnrollmentPeriod;

public class UpdateEnrollmentPeriodService(SykiDbContext ctx, HybridCache cache) : IAcademicService
{
    public async Task<OneOf<EnrollmentPeriodOut, SykiError>> Update(Guid institutionId, string id, UpdateEnrollmentPeriodIn data)
    {
        var academicPeriodExists = await ctx.AcademicPeriodExists(institutionId, id);
        if (!academicPeriodExists) return new AcademicPeriodNotFound();

        var period = await ctx.EnrollmentPeriods.FirstOrDefaultAsync(x => x.InstitutionId == institutionId && x.Id == id);
        if (period == null) return new EnrollmentPeriodNotFound();

        var result = period.Update(data.StartAt, data.EndAt);

        if (result.IsError()) return result.GetError();
        
        await ctx.SaveChangesAsync();

        await cache.RemoveAsync($"enrollmentPeriods:{institutionId}");

        return period.ToOut();
    }

    public async Task UpdateWithThrowOnError(Guid institutionId, string id, UpdateEnrollmentPeriodIn data)
    {
        (await Update(institutionId, id, data)).ThrowOnError();
    }
}
