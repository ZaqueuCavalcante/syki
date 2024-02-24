using Syki.Back.Database;
using Syki.Shared.CreateCampus;

namespace Syki.Back.CreateCampus;

public class CreateCampusService(SykiDbContext ctx)
{
    public async Task<CreateCampusOut> Create(Guid institutionId, CreateCampusIn data)
    {
        var campus = new Campus(institutionId, data.Name, data.City);

        ctx.Add(campus);
        await ctx.SaveChangesAsync();

        return campus.ToCreateCampusOut();
    }
}
