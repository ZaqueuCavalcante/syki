namespace Syki.Back.Features.Periods.GetAcademicPeriods;

public class GetAcademicPeriodsService(SykiDbContext ctx) : ISykiService
{
    public async Task<GetAcademicPeriodsOut> Get()
    {
        var data = await ctx.AcademicPeriods.AsNoTracking()
            .Where(p => p.InstitutionId == ctx.RequestUser.InstitutionId)
            .OrderBy(p => p.Id)
            .ToListAsync();

        return new GetAcademicPeriodsOut
        {
            Total = data.Count,
            Items = data.ConvertAll(p => p.ToGetAcademicPeriodsItemOut())
        };
    }
}
