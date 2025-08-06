namespace Syki.Back.Features.Teacher.AddStudentClassActivityNote;

public class AddStudentClassActivityNoteService(SykiDbContext ctx) : ITeacherService
{
    public async Task<OneOf<SykiSuccess, SykiError>> Add(Guid teacherId, Guid classActivityId, AddStudentClassActivityNoteIn data)
    {
        var activity = await ctx.ClassActivities.Where(x => x.Id == classActivityId).FirstOrDefaultAsync();
        if (activity == null) return new ClassActivityNotFound();

        var classOk = await ctx.Classes.AnyAsync(x => x.Id == activity.ClassId && x.TeacherId == teacherId);
        if (!classOk) return new ClassNotFound();

        var work = await ctx.ClassActivityWorks
            .Where(x => x.ClassActivityId == classActivityId && x.SykiStudentId == data.StudentId)
            .FirstOrDefaultAsync();
        if (work == null) return new ClassActivityNotFound();

        var result = work.AddNote(data.Value);
        if (result.IsError) return result.Error;

        await ctx.SaveChangesAsync();

        return new SykiSuccess();
    }
}
