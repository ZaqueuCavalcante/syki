namespace Syki.Back.Features.Teacher.GetTeacherCurrentClasses;

public class GetTeacherCurrentClassesService(SykiDbContext ctx) : ITeacherService
{
    public async Task<List<TeacherCurrentClassOut>> Get(Guid institutionId, Guid userId)
    {
        var classes = await ctx.Classes.AsNoTracking()
            .Include(t => t.Discipline)
            .Where(t => t.InstitutionId == institutionId && t.TeacherId == userId && t.Status == ClassStatus.Started)
            .ToListAsync();

        return classes.OrderBy(x => x.Discipline.Name).Select(t => t.ToTeacherCurrentClassOut()).ToList();
    }
}
