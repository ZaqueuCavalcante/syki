namespace Syki.Back.Features.Students.GetStudent;

public class GetStudentService(SykiDbContext ctx) : ISykiService
{
    public async Task<OneOf<GetStudentOut, SykiError>> Get(int studentId)
    {
        var institutionId = ctx.RequestUser.InstitutionId;

        var student = await ctx.Students.AsNoTracking()
            .Include(s => s.User)
            .FirstOrDefaultAsync(s => s.Id == studentId && s.InstitutionId == institutionId);

        if (student == null) return StudentNotFound.I;

        var currentEnrollment = await ctx.StudentCourseEnrollments.AsNoTracking()
            .Where(e => e.StudentId == studentId && e.LeftAt == null)
            .OrderByDescending(e => e.EnrolledAt)
            .FirstOrDefaultAsync();

        return new GetStudentOut
        {
            Id = student.Id,
            Name = student.Name,
            Email = student.User!.Email!,
            EnrollmentCode = student.EnrollmentCode,
            Status = student.Status,
            CurrentCourseOfferingId = currentEnrollment?.CourseOfferingId,
        };
    }
}
