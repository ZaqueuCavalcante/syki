namespace Syki.Back.Features.Student.GetStudentClassActivity;

public class GetStudentClassActivityService(SykiDbContext ctx) : ITeacherService
{
    public async Task<OneOf<StudentClassActivityOut, SykiError>> Get(Guid studentId, Guid classId, Guid activityId)
    {
        var classOk = await ctx.ClassesStudents.AnyAsync(x => x.ClassId == classId && x.SykiStudentId == studentId);
        if (!classOk) return new ClassNotFound();
        
        var activity = await ctx.ClassActivities.AsNoTracking()
            .Where(t => t.ClassId == classId && t.Id == activityId)
            .FirstOrDefaultAsync();
        if (activity == null) return new ClassActivityNotFound();

        var work = await ctx.ClassActivityWorks.AsNoTracking()
            .Where(x => x.ClassActivityId == activityId && x.SykiStudentId == studentId)
            .FirstAsync();

        return new StudentClassActivityOut
        {
            Id = activity.Id,
            ClassId = activity.ClassId,
            Note = activity.Note,
            Title = activity.Title,
            Description = activity.Description,
            Type = activity.Type,
            Status = activity.Status,
            Weight = activity.Weight,
            WorkStatus = work.Status,
            CreatedAt = activity.CreatedAt,
            DueDate = activity.DueDate,
            DueHour = activity.DueHour,
            Value = work.Note,
            PonderedValue = work.Note * activity.Weight / 100M,
        };
    }
}
