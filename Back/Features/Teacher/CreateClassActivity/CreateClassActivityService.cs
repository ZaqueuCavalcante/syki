namespace Syki.Back.Features.Teacher.CreateClassActivity;

public class CreateClassActivityService(SykiDbContext ctx) : ITeacherService
{
    public async Task<OneOf<SykiSuccess, SykiError>> Create(Guid teacherId, Guid classId, CreateClassActivityIn data)
    {
        var @class = await ctx.Classes
            .Include(x => x.Students)
            .FirstOrDefaultAsync(x => x.Id == classId && x.TeacherId == teacherId);
        if (@class == null) return new ClassNotFound();

        var classActivity = new ClassActivity(
            classId,
            data.Title,
            data.Description,
            data.DueDate
        );

        ctx.Add(classActivity);
        await ctx.SaveChangesAsync();

        return new SykiSuccess();
    }
}
