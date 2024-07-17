namespace Syki.Back.Features.Academic.StartClass;

public class StartClassService(SykiDbContext ctx)
{
    public async Task Start(Guid institutionId, Guid classId)
    {
        var @class = await ctx.Classes
            .Include(x => x.Students)
            .FirstOrDefaultAsync(x => x.InstitutionId == institutionId && x.Id == classId);

        if (@class == null)
            Throw.DE028.Now();
        
        @class.Start();

        await ctx.SaveChangesAsync();
    }
}
