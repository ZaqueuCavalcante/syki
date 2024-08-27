namespace Syki.Back.Features.Academic.GetAcademicPeriods;

public class GetAcademicPeriodsService(SykiDbContext ctx) : IAcademicService
{
    public async Task<List<AcademicPeriodOut>> Get(Guid institutionId)
    {
        var periods = await ctx.AcademicPeriods.AsNoTracking()
            .Where(c => c.InstitutionId == institutionId)
            .OrderBy(p => p.Id)
            .ToListAsync();
        
        return periods.ConvertAll(p => p.ToOut());
    }
}
