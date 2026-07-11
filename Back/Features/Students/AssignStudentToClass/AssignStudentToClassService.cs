using Estud.Back.Domain.Classes;

namespace Estud.Back.Features.Students.AssignStudentToClass;

public class AssignStudentToClassService(EstudDbContext ctx) : IEstudService
{
    public async Task<OneOf<EstudSuccess, EstudError>> Assign(int studentId, AssignStudentToClassIn data)
    {
        var institutionId = ctx.RequestUser.InstitutionId;

        var student = await ctx.Students.FirstOrDefaultAsync(s => s.Id == studentId && s.InstitutionId == institutionId);
        if (student == null) return StudentNotFound.I;

        var @class = await ctx.Classes.FirstOrDefaultAsync(c => c.Id == data.ClassId && c.InstitutionId == institutionId);
        if (@class == null) return ClassNotFound.I;

        if (@class.Status != ClassStatus.OnEnrollment) return ClassMustBeOnEnrollment.I;

        var alreadyEnrolled = await ctx.ClassStudents.AnyAsync(x => x.ClassId == @class.Id && x.StudentId == studentId);
        if (alreadyEnrolled) return StudentAlreadyEnrolledInClass.I;

        var enrolled = await ctx.ClassStudents.CountAsync(x => x.ClassId == @class.Id);
        if (enrolled >= @class.Vacancies) return NoVacanciesInClass.I;

        ctx.Add(new ClassStudent(@class.Id, studentId));
        await ctx.SaveChangesAsync();

        return EstudSuccess.I;
    }
}
