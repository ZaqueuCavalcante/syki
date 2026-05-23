using Syki.Back.Domain.Periods;

namespace Syki.Back.Features.Periods.CreateAcademicPeriod;

public class CreateAcademicPeriodService(SykiDbContext ctx) : ISykiService
{
    public async Task<OneOf<CreateAcademicPeriodOut, SykiError>> Create(CreateAcademicPeriodIn data)
    {
        var institutionId = ctx.RequestUser.InstitutionId;

        var result = AcademicPeriod.New(institutionId, data.Name, data.StartAt, data.EndAt);
        if (result.IsError) return result.Error;

        var period = result.Success;

        var periodExists = await ctx.AcademicPeriods.AnyAsync(p => p.InstitutionId == institutionId && p.Name == period.Name);
        if (periodExists) return AcademicPeriodAlreadyExists.I;

        await ctx.SaveChangesAsync(period);

        return new CreateAcademicPeriodOut { Id = period.Id };
    }
}
