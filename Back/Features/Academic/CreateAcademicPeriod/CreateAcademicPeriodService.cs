namespace Syki.Back.Features.Academic.CreateAcademicPeriod;

public class CreateAcademicPeriodService(SykiDbContext ctx)
{
    public async Task<OneOf<AcademicPeriodOut, SykiError>> Create(Guid institutionId, CreateAcademicPeriodIn data)
    {
        var periodExists = await ctx.AcademicPeriods.AnyAsync(p => p.InstitutionId == institutionId && p.Id == data.Id);
        if (periodExists) return new AcademicPeriodAlreadyExists();

        var result = AcademicPeriod.New(data.Id, institutionId, data.StartAt, data.EndAt);

        return await result.Match<Task<OneOf<AcademicPeriodOut, SykiError>>>(
            async period =>
            {
                ctx.Add(period);
                await ctx.SaveChangesAsync();
                return period.ToOut();
            },
            error => Task.FromResult<OneOf<AcademicPeriodOut, SykiError>>(error)
        );
    }
}
