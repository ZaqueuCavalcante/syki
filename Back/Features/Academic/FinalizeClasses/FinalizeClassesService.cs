namespace Syki.Back.Features.Academic.FinalizeClasses;

public class FinalizeClassesService(SykiDbContext ctx) : IAcademicService
{
    public async Task<OneOf<SykiSuccess, SykiError>> Finalize(Guid institutionId, FinalizeClassesIn data)
    {
        var classes = await ctx.Classes
            .Include(x => x.Lessons)
            .Where(c => c.InstitutionId == institutionId && data.Classes.Contains(c.Id))
            .ToListAsync();

        if (classes.Count == 0) return new InvalidClassesList();
        
        var statusOk = classes.All(x => x.Status == ClassStatus.Started);
        if (!statusOk) return new ClassMustHaveStartedStatus();

        var results = classes.ConvertAll(c => c.Finish());
        foreach (var result in results)
        {
            if (result.IsError()) return result.GetError();
        }

        await ctx.SaveChangesAsync();

        return new SykiSuccess();
    }
}
