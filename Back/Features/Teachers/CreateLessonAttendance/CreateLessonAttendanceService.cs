using Estud.Back.Domain.Classes;

namespace Estud.Back.Features.Teachers.CreateLessonAttendance;

public class CreateLessonAttendanceService(EstudDbContext ctx) : IEstudService
{
    public async Task<OneOf<EstudSuccess, EstudError>> Create(int id, CreateLessonAttendanceIn data)
    {
        var userId = ctx.RequestUser.Id;
        var institutionId = ctx.RequestUser.InstitutionId;
        var teacherId = await ctx.GetTeacherId(institutionId, userId);

        var lesson = await ctx.ClassLessons
            .Include(l => l.Attendances)
            .FirstOrDefaultAsync(l => l.Id == id && l.Class.InstitutionId == institutionId);
        if (lesson == null) return ClassLessonNotFound.I;

        var assigned = await ctx.ClassTeachers.AnyAsync(ct => ct.ClassId == lesson.ClassId && ct.TeacherId == teacherId);
        if (!assigned) return TeacherNotAssignedToClass.I;

        var students = await ctx.ClassStudents.AsNoTracking()
            .Where(cs => cs.ClassId == lesson.ClassId && cs.Status == StudentClassStatus.Matriculado)
            .Select(cs => cs.StudentId).ToListAsync();

        if (!data.PresentStudents.IsSubsetOf(students)) return InvalidStudentsList.I;

        foreach (var studentId in students)
        {
            var present = data.PresentStudents.Contains(studentId);
            var attendance = lesson.Attendances.FirstOrDefault(a => a.StudentId == studentId);
            if (attendance == null)
            {
                ctx.Add(new ClassLessonAttendance(lesson.ClassId, lesson.Id, studentId, present));
            }
            else
            {
                attendance.Update(present);
            }
        }

        lesson.Finish();

        await ctx.SaveChangesAsync();

        return EstudSuccess.I;
    }
}
