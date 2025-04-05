namespace Syki.Back.Features.Teacher.GetTeacherClassActivities;

public class GetTeacherClassActivitiesService(SykiDbContext ctx) : ITeacherService
{
    public async Task<OneOf<List<TeacherClassActivityOut>, SykiError>> Get(Guid teacherId, Guid classId)
    {
        var classOk = await ctx.Classes.AnyAsync(x => x.Id == classId && x.TeacherId == teacherId);
        if (!classOk) return new ClassNotFound();

        var activities = await ctx.ClassActivities.AsNoTracking()
            .Where(t => t.ClassId == classId)
            .OrderByDescending(x => x.CreatedAt)
            .ToListAsync();

        return activities.ConvertAll(x => x.ToOut());
    }
}
