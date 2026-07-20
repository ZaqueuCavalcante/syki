namespace Estud.Back.Features.Teacher.GetTeacherClassActivity;

public class GetTeacherClassActivityService(EstudDbContext ctx) : IEstudService
{
    public async Task<OneOf<TeacherClassActivityOut, EstudError>> Get(Guid teacherId, Guid classId, Guid activityId)
    {
        var classOk = await ctx.Classes.AnyAsync(x => x.Id == classId && x.TeacherId == teacherId);
        if (!classOk) return new ClassNotFound();

        var activity = await ctx.ClassActivities.AsNoTracking()
            .Include(x => x.Works)
                .ThenInclude(w => w.EstudStudent)
            .Where(t => t.ClassId == classId && t.Id == activityId)
            .FirstOrDefaultAsync();

        if (activity == null) return new ClassActivityNotFound();

        return activity.ToOut();
    }
}
