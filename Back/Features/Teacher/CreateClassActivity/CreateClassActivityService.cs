namespace Syki.Back.Features.Teacher.CreateClassActivity;

public class CreateClassActivityService(SykiDbContext ctx) : ITeacherService
{
    public async Task<OneOf<SykiSuccess, SykiError>> Create(Guid teacherId, Guid classId, CreateClassActivityIn data)
    {
        var classOk = await ctx.Classes.AnyAsync(x => x.Id == classId && x.TeacherId == teacherId);
        if (!classOk) return new ClassNotFound();

        var lessonOk = await ctx.Lessons.AnyAsync(x => x.Id == data.LessonId && x.ClassId == classId);
        if (!lessonOk) return new LessonNotFound();

        var classActivity = new ClassActivity(
            classId,
            data.LessonId,
            data.Title,
            data.Description,
            data.DueDate
        );

        ctx.Add(classActivity);
        await ctx.SaveChangesAsync();

        return new SykiSuccess();
    }
}
