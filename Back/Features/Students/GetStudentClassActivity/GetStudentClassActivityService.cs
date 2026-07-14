namespace Estud.Back.Features.Students.GetStudentClassActivity;

public class GetStudentClassActivityService(EstudDbContext ctx) : IEstudService
{
    public async Task<OneOf<GetStudentClassActivityOut, EstudError>> Get(int id, int activityId)
    {
        var userId = ctx.RequestUser.Id;
        var institutionId = ctx.RequestUser.InstitutionId;
        var studentId = await ctx.GetStudentId(institutionId, userId);

        var classOk = await ctx.Classes.AsNoTracking().AnyAsync(c => c.Id == id && c.InstitutionId == institutionId);
        if (!classOk) return ClassNotFound.I;

        var enrolled = await ctx.ClassStudents.AsNoTracking().AnyAsync(cs => cs.ClassId == id && cs.StudentId == studentId);
        if (!enrolled) return StudentNotEnrolledInClass.I;

        var activity = await ctx.ClassActivities.AsNoTracking()
            .Where(a => a.Id == activityId && a.ClassId == id)
            .Select(a => new
            {
                Activity = a,
                Work = a.Works.FirstOrDefault(w => w.StudentId == studentId),
            })
            .FirstOrDefaultAsync();
        if (activity == null) return ClassActivityNotFound.I;

        return activity.Activity.ToGetStudentClassActivityOut(activity.Work);
    }
}
