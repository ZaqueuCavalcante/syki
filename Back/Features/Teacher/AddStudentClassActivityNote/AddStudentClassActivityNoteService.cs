namespace Syki.Back.Features.Teacher.AddStudentClassActivityNote;

public class AddStudentClassActivityNoteService(SykiDbContext ctx) : ITeacherService
{
    public async Task<OneOf<SykiSuccess, SykiError>> Add(Guid teacherId, Guid classActivityId, AddStudentClassActivityNoteIn data)
    {
        var activity = await ctx.ClassActivities.Where(x => x.Id == classActivityId).FirstOrDefaultAsync();
        if (activity == null) return new ClassActivityNotFound();

        var classOk = await ctx.Classes.AnyAsync(x => x.Id == activity.ClassId && x.TeacherId == teacherId);
        if (!classOk) return new ClassNotFound();

        var result = StudentClassActivityNote.New(data.StudentId, classActivityId, data.Value);
        if (result.IsError()) return result.GetError();

        ctx.Add(result.GetSuccess());
        await ctx.SaveChangesAsync();

        return new SykiSuccess();
    }
}
