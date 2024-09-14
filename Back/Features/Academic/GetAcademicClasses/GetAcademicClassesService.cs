namespace Syki.Back.Features.Academic.GetAcademicClasses;

public class GetAcademicClassesService(SykiDbContext ctx) : IAcademicService
{
    public async Task<List<ClassOut>> Get(Guid institutionId, GetAcademicClassesIn query)
    {
        var status = query?.Status ?? null;
        var classes = await ctx.Classes.AsNoTracking()
            .Include(t => t.Discipline)
            .Include(t => t.Teacher)
            .Include(t => t.Schedules)
            .Include(t => t.Lessons)
                .ThenInclude(l => l.Attendances)
            .Where(c => c.InstitutionId == institutionId && (status == null || c.Status == status))
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
