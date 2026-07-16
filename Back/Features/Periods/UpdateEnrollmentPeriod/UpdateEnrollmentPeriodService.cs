namespace Estud.Back.Features.Periods.UpdateEnrollmentPeriod;

public class UpdateEnrollmentPeriodService(EstudDbContext ctx) : IEstudService
{
    public async Task<OneOf<UpdateEnrollmentPeriodOut, EstudError>> Update(UpdateEnrollmentPeriodIn data)
    {
        var institutionId = ctx.RequestUser.InstitutionId;

        var period = await ctx.EnrollmentPeriods.FirstOrDefaultAsync(p => p.InstitutionId == institutionId && p.Id == data.Id);
        if (period == null) return EnrollmentPeriodNotFound.I;

        var nameExists = await ctx.EnrollmentPeriods.AnyAsync(p => p.InstitutionId == institutionId && p.Name == data.Name && p.Id != data.Id);
        if (nameExists) return EnrollmentPeriodAlreadyExists.I;

        var result = period.Update(data.Name, data.StartAt, data.EndAt);
        if (result.IsError) return result.Error;

        await ctx.SaveChangesAsync();

        return period.ToUpdateEnrollmentPeriodOut();
    }
}
