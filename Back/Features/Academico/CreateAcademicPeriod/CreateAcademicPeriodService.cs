namespace Syki.Back.CreateAcademicPeriod;

public class CreateAcademicPeriodService(SykiDbContext ctx)
{
    public async Task<AcademicPeriodOut> Create(Guid institutionId, CreateAcademicPeriodIn data)
    {
        var periodExists = await ctx.AcademicPeriods.AnyAsync(p => p.InstitutionId == institutionId && p.Id == data.Id);
        if (periodExists)
            Throw.DE026.Now();

        var period = new AcademicPeriod(data.Id, institutionId, data.Start, data.End);

        ctx.Add(period);
        await ctx.SaveChangesAsync();

        return period.ToOut();
    }
}
