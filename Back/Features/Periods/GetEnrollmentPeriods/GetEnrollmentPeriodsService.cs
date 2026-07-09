namespace Estud.Back.Features.Periods.GetEnrollmentPeriods;

public class GetEnrollmentPeriodsService(EstudDbContext ctx) : IEstudService
{
    public async Task<GetEnrollmentPeriodsOut> Get()
    {
        var data = await ctx.EnrollmentPeriods.AsNoTracking()
            .Where(p => p.InstitutionId == ctx.RequestUser.InstitutionId)
            .OrderBy(p => p.Id)
            .ToListAsync();

        return new GetEnrollmentPeriodsOut
        {
            Total = data.Count,
            Items = data.ConvertAll(p => p.ToGetEnrollmentPeriodsItemOut())
        };
    }
}
