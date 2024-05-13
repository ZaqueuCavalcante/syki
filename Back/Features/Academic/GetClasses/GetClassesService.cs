namespace Syki.Back.Features.Academic.GetClasses;

public class GetClassesService(SykiDbContext ctx)
{
    public async Task<List<ClassOut>> Get(Guid institutionId)
    {
        var classes = await ctx.Classes.AsNoTracking()
            .Include(t => t.Discipline)
            .Include(t => t.Teacher)
            .Include(t => t.Schedules)
            .Where(c => c.InstitutionId == institutionId)
            .ToListAsync();

        return classes.ConvertAll(t => t.ToOut());
    }
}
