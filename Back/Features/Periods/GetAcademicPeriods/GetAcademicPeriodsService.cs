namespace Estud.Back.Features.Periods.GetAcademicPeriods;

public class GetAcademicPeriodsService(EstudDbContext ctx) : IEstudService
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
