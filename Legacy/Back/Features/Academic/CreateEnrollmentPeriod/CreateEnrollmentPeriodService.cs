namespace Estud.Back.Features.Academic.CreateEnrollmentPeriod;

public class CreateEnrollmentPeriodService(EstudDbContext ctx, HybridCache cache) : IEstudService
{
    public async Task<OneOf<EnrollmentPeriodOut, EstudError>> Create(Guid institutionId, CreateEnrollmentPeriodIn data)
    {
        if (await ctx.AcademicPeriodNotFound(data.Id)) return AcademicPeriodNotFound.I;

        var enrollmentPeriodExists = await ctx.EnrollmentPeriods.AnyAsync(x => x.InstitutionId == institutionId && x.Id == data.Id);
        if (enrollmentPeriodExists) return new EnrollmentPeriodAlreadyExists();
        
        var result = EnrollmentPeriod.New(data.Id, institutionId, data.StartAt, data.EndAt);

        if (result.IsError) return result.Error;

        var period = result.Success;
        ctx.Add(period);
        await ctx.SaveChangesAsync();

        await cache.RemoveAsync($"enrollmentPeriods:{institutionId}");

        return period.ToOut();
    }
}
