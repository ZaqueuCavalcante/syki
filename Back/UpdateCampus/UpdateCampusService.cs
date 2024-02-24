using Syki.Back.Database;
using Syki.Back.Exceptions;
using Syki.Shared.UpdateCampus;
using Microsoft.EntityFrameworkCore;

namespace Syki.Back.UpdateCampus;

public class UpdateCampusService(SykiDbContext ctx)
{
    public async Task<UpdateCampusOut> Update(Guid institutionId, UpdateCampusIn data)
    {
        var campus = await ctx.Campi
            .FirstOrDefaultAsync(x => x.InstitutionId == institutionId && x.Id == data.Id);

        if (campus == null)
            Throw.DE010.Now();

        campus.Update(data.Name, data.City);
        await ctx.SaveChangesAsync();

        return campus.ToUpdateCampusOut();
    }
}
