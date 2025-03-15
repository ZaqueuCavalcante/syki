namespace Syki.Back.Features.Teacher.CreateClassActivity;

public class CreateClassActivityService(SykiDbContext ctx) : ITeacherService
{
    public async Task<OneOf<SykiSuccess, SykiError>> Create(Guid teacherId, Guid classId, CreateClassActivityIn data)
    {
        var classOk = await ctx.Classes.AnyAsync(x => x.Id == classId && x.TeacherId == teacherId);
        if (!classOk) return new ClassNotFound();

        var classActivity = new ClassActivity(
            classId,
            data.Title,
            data.Description,
            data.DueDate,
            data.DueHour
        );

        ctx.Add(classActivity);
        await ctx.SaveChangesAsync();

        return new SykiSuccess();
    }
}
