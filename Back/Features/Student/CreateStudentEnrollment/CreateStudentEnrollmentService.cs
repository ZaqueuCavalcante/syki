using Syki.Back.Features.Academic.StartClass;

namespace Syki.Back.Features.Student.CreateStudentEnrollment;

public class CreateStudentEnrollmentService(SykiDbContext ctx)
{
    public async Task Create(Guid institutionId, Guid userId, CreateStudentEnrollmentIn data)
    {
        var ids = await ctx.Classes
            .Where(t => t.InstitutionId == institutionId && data.Classes.Contains(t.Id))
            .Select(t => t.Id)
            .ToListAsync();

        var classesStudents = await ctx.ClassesStudents.Where(x => x.SykiStudentId == userId).ToListAsync();
        ctx.RemoveRange(classesStudents);
        ids.ForEach(id => ctx.Add(new ClassStudent(id, userId, StudentDisciplineStatus.Matriculado)));

        var examGrades = await ctx.ExamGrades.Where(x => x.StudentId == userId).ToListAsync();
        ctx.RemoveRange(examGrades);
        ids.ForEach(id =>
        {
            Enum.GetValues<ExamType>().ToList().ForEach(type => ctx.Add(new ExamGrade(id, userId, type)));
        });

        await ctx.SaveChangesAsync();
    }
}
