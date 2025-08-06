namespace Syki.Back.Features.Teacher.CreateClassActivity;

public class CreateClassActivityService(SykiDbContext ctx) : ITeacherService
{
    public async Task<OneOf<CreateClassActivityOut, SykiError>> Create(Guid teacherId, Guid classId, CreateClassActivityIn data)
    {
        var @class = await ctx.Classes
            .Include(x => x.Activities)
            .FirstOrDefaultAsync(x => x.Id == classId && x.TeacherId == teacherId);
        if (@class == null) return new ClassNotFound();

        var students = await ctx.ClassesStudents.Where(x => x.ClassId == classId)
            .Select(x => x.SykiStudentId).ToListAsync();

        var activity = ClassActivity.New(
            classId,
            data.Note,
            data.Title,
            data.Description,
            data.Type,
            data.Weight,
            data.DueDate,
            data.DueHour,
            students
        );
        if (activity.IsError) return activity.Error;

        var result = @class.AddActivity(activity.Success);
        if (result.IsError) return result.Error;

        await ctx.SaveChangesAsync();

        return activity.Success.ToCreateOut();
    }
}
