namespace Syki.Back.Features.Teacher.CreateLessonAttendance;

public class CreateLessonAttendanceService(SykiDbContext ctx) : ITeacherService
{
    public async Task<OneOf<SykiSuccess, SykiError>> Create(Guid teacherId, Guid lessonId, CreateLessonAttendanceIn data)
    {
        var lesson = await ctx.Lessons.FirstOrDefaultAsync(x => x.Id == lessonId);
        if (lesson == null) return new LessonNotFound();

        var @class = await ctx.Classes
            .Include(x => x.Students)
            .FirstOrDefaultAsync(x => x.Id == lesson.ClassId && x.TeacherId == teacherId);
        if (@class == null) return new LessonNotFound();

        var allStudents = @class.Students.Select(x => x.Id).ToList();
        if (!data.PresentStudents.IsSubsetOf(allStudents))
            return new InvalidStudentsList();

        foreach (var studentId in allStudents)
        {
            var present = data.PresentStudents.Contains(studentId);
            var attendance = new LessonAttendance(@class.Id, lesson.Id, studentId, present);
            ctx.Add(attendance);
        }

        await ctx.SaveChangesAsync();

        return new SykiSuccess();
    }
}
