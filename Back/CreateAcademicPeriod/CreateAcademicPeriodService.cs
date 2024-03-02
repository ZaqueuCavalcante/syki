namespace Syki.Back.CreateAcademicPeriod;

public class CreateAcademicPeriodService(SykiDbContext ctx)
{
    public async Task<AcademicPeriodOut> Create(Guid institutionId, CreateAcademicPeriodIn data)
    {
        var periodoExists = await ctx.AcademicPeriods
            .AnyAsync(p => p.InstitutionId == institutionId && p.Id == data.Id);
        if (periodoExists)
            Throw.DE026.Now();

        var periodo = new AcademicPeriod(data.Id, institutionId, data.Start, data.End);

        ctx.Add(periodo);
        await ctx.SaveChangesAsync();

        return periodo.ToOut();
    }
}
