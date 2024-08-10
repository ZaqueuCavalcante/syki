namespace Syki.Back.Features.Academic.StartClass;

public class StartClassService(SykiDbContext ctx) : IAcademicService
{
    public async Task<OneOf<SykiSuccess, SykiError>> Start(Guid institutionId, Guid classId)
    {
        var @class = await ctx.Classes
            .Include(x => x.Students)
            .FirstOrDefaultAsync(x => x.InstitutionId == institutionId && x.Id == classId);

        if (@class == null) return new ClassNotFound();
        
        @class.Start();

        await ctx.SaveChangesAsync();

        return new SykiSuccess();
    }
}
