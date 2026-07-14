namespace Estud.Back.Features.Teachers.GetTeacherClassActivity;

public class GetTeacherClassActivityService(EstudDbContext ctx) : IEstudService
{
    public async Task<OneOf<GetTeacherClassActivityOut, EstudError>> Get(int classId, int activityId)
    {
        var userId = ctx.RequestUser.Id;
        var institutionId = ctx.RequestUser.InstitutionId;
        var teacherId = await ctx.GetTeacherId(institutionId, userId);

        var @class = await ctx.Classes.AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == classId && c.InstitutionId == institutionId);
        if (@class == null) return ClassNotFound.I;

        if (@class.TeacherId != teacherId) return TeacherNotAssignedToClass.I;

        var activity = await ctx.ClassActivities.AsNoTracking()
            .Include(a => a.Works).ThenInclude(w => w.Student)
            .FirstOrDefaultAsync(a => a.Id == activityId && a.ClassId == classId);
        if (activity == null) return ClassActivityNotFound.I;

        return activity.ToGetTeacherClassActivityOut();
    }
}
