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
        
        var ids = classes.ConvertAll(x => x.Id);
        var links = await ctx.ClassesStudents.Where(x => ids.Contains(x.ClassId)).ToListAsync();
        foreach (var @class in classes)
        {
            var count = links.Count(x => x.ClassId == @class.Id);
            @class.SetFillRatio(count);
        }

        return classes.ConvertAll(t => t.ToOut());
    }
}
