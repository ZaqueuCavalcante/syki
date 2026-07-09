using Estud.Back.Domain.Periods;

namespace Estud.Back.Features.Periods.CreateEnrollmentPeriod;

public class CreateEnrollmentPeriodService(EstudDbContext ctx) : IEstudService
{
    public async Task<OneOf<CreateEnrollmentPeriodOut, EstudError>> Create(CreateEnrollmentPeriodIn data)
    {
        var institutionId = ctx.RequestUser.InstitutionId;

        var result = EnrollmentPeriod.New(institutionId, data.Name, data.StartAt, data.EndAt);
        if (result.IsError) return result.Error;

        var period = result.Success;

        var periodExists = await ctx.EnrollmentPeriods.AnyAsync(p => p.InstitutionId == institutionId && p.Name == period.Name);
        if (periodExists) return EnrollmentPeriodAlreadyExists.I;

        await ctx.SaveChangesAsync(period);

        return new CreateEnrollmentPeriodOut { Id = period.Id };
    }
}
