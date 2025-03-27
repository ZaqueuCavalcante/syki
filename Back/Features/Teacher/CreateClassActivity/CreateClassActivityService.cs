namespace Syki.Back.Features.Teacher.CreateClassActivity;

public class CreateClassActivityService(SykiDbContext ctx) : ITeacherService
{
    public async Task<OneOf<SykiSuccess, SykiError>> Create(Guid teacherId, Guid classId, CreateClassActivityIn data)
    {
        var @class = await ctx.Classes.Include(x => x.Activities)
            .FirstOrDefaultAsync(x => x.Id == classId && x.TeacherId == teacherId);
        if (@class == null) return new ClassNotFound();

        var activity = ClassActivity.New(
            classId,
            data.Note,
            data.Title,
            data.Description,
            data.Type,
            data.Weight,
            data.DueDate,
            data.DueHour
        );
        if (activity.IsError()) return activity.GetError();

        var result = @class.AddActivity(activity.GetSuccess());
        if (result.IsError()) return result.GetError();

        await ctx.SaveChangesAsync();

        return new SykiSuccess();
    }
}
