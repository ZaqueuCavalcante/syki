namespace Syki.Back.Features.Student.CreateStudentEnrollment;

public class CreateStudentEnrollmentService(SykiDbContext ctx)
{
    public async Task Create(Guid institutionId, Guid userId, CreateStudentEnrollmentIn data)
    {
        var ids = await ctx.Classes
            .Where(t => t.InstitutionId == institutionId && data.Classes.Contains(t.Id))
            .Select(t => t.Id)
            .ToListAsync();

        var classes = await ctx.ClassesStudents.Where(x => x.StudentId == userId).ToListAsync();
        ctx.RemoveRange(classes);

        foreach (var id in ids)
        {
            ctx.Add(new ClassStudent(id, userId, StudentDisciplineStatus.Matriculado));
        }

        await ctx.SaveChangesAsync();
    }
}
