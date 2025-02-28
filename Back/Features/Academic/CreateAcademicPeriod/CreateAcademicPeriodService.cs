namespace Syki.Back.Features.Academic.CreateAcademicPeriod;

public class CreateAcademicPeriodService(SykiDbContext ctx, HybridCache cache) : IAcademicService
{
    public async Task<OneOf<AcademicPeriodOut, SykiError>> Create(Guid institutionId, CreateAcademicPeriodIn data)
    {
        var result = AcademicPeriod.New(data.Id, institutionId, data.StartAt, data.EndAt);
        if (result.IsError()) return result.GetError();

        var period = result.GetSuccess();

        var periodExists = await ctx.AcademicPeriods.AnyAsync(p => p.InstitutionId == institutionId && p.Id == period.Id);
        if (periodExists) return new AcademicPeriodAlreadyExists();

        ctx.Add(period);
        await ctx.SaveChangesAsync();

        await cache.RemoveAsync($"academicPeriods:{institutionId}");

        return period.ToOut();
    }
}
