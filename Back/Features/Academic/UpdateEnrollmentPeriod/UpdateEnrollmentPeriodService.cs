namespace Syki.Back.Features.Academic.UpdateEnrollmentPeriod;

public class UpdateEnrollmentPeriodService(SykiDbContext ctx) : IAcademicService
{
    public async Task<OneOf<EnrollmentPeriodOut, SykiError>> Update(Guid institutionId, string id, UpdateEnrollmentPeriodIn data)
    {
        var academicPeriodExists = await ctx.AcademicPeriodExists(institutionId, id);
        if (!academicPeriodExists) return new AcademicPeriodNotFound();

        var period = await ctx.EnrollmentPeriods.FirstOrDefaultAsync(x => x.InstitutionId == institutionId && x.Id == id);
        if (period == null) return new EnrollmentPeriodNotFound();

        period.Update(data.StartAt, data.EndAt);
        
        await ctx.SaveChangesAsync();

        return period.ToOut();
    }
}
