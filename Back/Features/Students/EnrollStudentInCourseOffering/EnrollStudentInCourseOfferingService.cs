using Estud.Back.Domain.Students;

namespace Estud.Back.Features.Students.EnrollStudentInCourseOffering;

public class EnrollStudentInCourseOfferingService(EstudDbContext ctx) : IEstudService
{
    public async Task<OneOf<EnrollStudentInCourseOfferingOut, EstudError>> Enroll(int studentId, EnrollStudentInCourseOfferingIn data)
    {
        var institutionId = ctx.RequestUser.InstitutionId;

        var student = await ctx.Students.FirstOrDefaultAsync(s => s.Id == studentId && s.InstitutionId == institutionId);
        if (student == null) return StudentNotFound.I;

        var offering = await ctx.CourseOfferings.FirstOrDefaultAsync(o => o.Id == data.CourseOfferingId && o.InstitutionId == institutionId);
        if (offering == null) return CourseOfferingNotFound.I;

        var alreadyEnrolled = await ctx.StudentCourseEnrollments
            .AnyAsync(e => e.StudentId == studentId && e.CourseOfferingId == data.CourseOfferingId);
        if (alreadyEnrolled) return StudentAlreadyEnrolledInCourseOffering.I;

        var enrollment = new StudentCourseEnrollment(studentId, data.CourseOfferingId);
        ctx.Add(enrollment);
        await ctx.SaveChangesAsync();

        return new EnrollStudentInCourseOfferingOut { Id = enrollment.Id };
    }
}
