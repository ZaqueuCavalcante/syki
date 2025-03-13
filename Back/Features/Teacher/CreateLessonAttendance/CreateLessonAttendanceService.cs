namespace Syki.Back.Features.Teacher.CreateLessonAttendance;

public class CreateLessonAttendanceService(SykiDbContext ctx) : ITeacherService
{
    public async Task<OneOf<SykiSuccess, SykiError>> Create(Guid teacherId, Guid lessonId, CreateLessonAttendanceIn data)
    {
        var lesson = await ctx.Lessons.Include(l => l.Attendances).FirstOrDefaultAsync(x => x.Id == lessonId);
        if (lesson == null) return new LessonNotFound();

        var @class = await ctx.Classes
            .Include(x => x.Students)
            .FirstOrDefaultAsync(x => x.Id == lesson.ClassId && x.TeacherId == teacherId);
        if (@class == null) return new ClassNotFound();

        var allStudents = @class.Students.Select(x => x.Id).ToList();
        if (!data.PresentStudents.IsSubsetOf(allStudents))
            return new InvalidStudentsList();

        var attendances = lesson.Attendances;
        foreach (var studentId in allStudents)
        {
            var present = data.PresentStudents.Contains(studentId);
            var attendance = attendances.FirstOrDefault(a => a.StudentId == studentId);
            if (attendance != null)
            {
                attendance.Update(present);
            }
            else
            {
                attendance = new LessonAttendance(@class.Id, lesson.Id, studentId, present);
                ctx.Add(attendance);
            }
        }

        lesson.Finish();

        await ctx.SaveChangesAsync();

        return new SykiSuccess();
    }

    public async Task CreateWithThrowOnError(Guid teacherId, Guid lessonId, CreateLessonAttendanceIn data)
    {
        (await Create(teacherId, lessonId, data)).ThrowOnError();
    }
}
