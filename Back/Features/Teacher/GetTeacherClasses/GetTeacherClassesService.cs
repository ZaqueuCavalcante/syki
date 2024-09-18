namespace Syki.Back.Features.Teacher.GetTeacherClasses;

public class GetTeacherClassesService(SykiDbContext ctx) : ITeacherService
{
    public async Task<List<TeacherClassesOut>> Get(Guid institutionId, Guid userId)
    {
        var classes = await ctx.Classes.AsNoTracking()
            .Include(t => t.Discipline)
            .Include(t => t.Schedules)
            .Where(t => t.InstitutionId == institutionId && t.TeacherId == userId && t.Status == ClassStatus.Started)
            .ToListAsync();

        return classes.OrderBy(x => x.Discipline.Name).Select(t => t.ToTeacherClassesOut()).ToList();
    }
}
