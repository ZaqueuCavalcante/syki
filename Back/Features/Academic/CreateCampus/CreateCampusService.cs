namespace Syki.Back.Features.Academic.CreateCampus;

public class CreateCampusService(SykiDbContext ctx) : IAcademicService
{
    public async Task<CampusOut> Create(Guid institutionId, CreateCampusIn data)
    {
        var campus = new Campus(institutionId, data.Name, data.City);

        ctx.Add(campus);
        await ctx.SaveChangesAsync();

        return campus.ToOut();
    }
}
