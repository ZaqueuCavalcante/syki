namespace Syki.Back.Features.Academic.CreateCampus;

public class CreateCampusService(SykiDbContext ctx, HybridCache cache) : IAcademicService
{
    public async Task<CampusOut> Create(Guid institutionId, CreateCampusIn data)
    {
        var campus = new Campus(institutionId, data.Name, data.State, data.City);

        ctx.Add(campus);
        await ctx.SaveChangesAsync();

        await cache.RemoveAsync($"campi:{institutionId}");

        return campus.ToOut();
    }
}
