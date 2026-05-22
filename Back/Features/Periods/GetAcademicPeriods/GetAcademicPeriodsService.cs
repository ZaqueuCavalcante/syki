namespace Syki.Back.Features.Periods.GetAcademicPeriods;

public class GetAcademicPeriodsService(SykiDbContext ctx) : ISykiService
{
    public async Task<List<GetAcademicPeriodsItemOut>> Get()
    {
        var data = await ctx.AcademicPeriods.AsNoTracking()
            .Where(p => p.InstitutionId == ctx.RequestUser.InstitutionId)
            .OrderBy(p => p.Id)
            .ToListAsync();

        return data.ConvertAll(p => p.ToGetAcademicPeriodsItemOut());
    }
}
