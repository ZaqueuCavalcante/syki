namespace Estud.Back.Features.Academic.UpdateEnrollmentPeriod;

public class UpdateEnrollmentPeriodService(EstudDbContext ctx, HybridCache cache) : IEstudService
{
    public async Task<OneOf<EnrollmentPeriodOut, EstudError>> Update(Guid institutionId, string id, UpdateEnrollmentPeriodIn data)
    {
        if (await ctx.AcademicPeriodNotFound(id)) return AcademicPeriodNotFound.I;

        var period = await ctx.EnrollmentPeriods.FirstOrDefaultAsync(x => x.InstitutionId == institutionId && x.Id == id);
        if (period == null) return new EnrollmentPeriodNotFound();

        var result = period.Update(data.StartAt, data.EndAt);

        if (result.IsError) return result.Error;
        
        await ctx.SaveChangesAsync();

        await cache.RemoveAsync($"enrollmentPeriods:{institutionId}");

        return period.ToOut();
    }
}
