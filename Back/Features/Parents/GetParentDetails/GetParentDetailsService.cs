namespace Estud.Back.Features.Parents.GetParentDetails;

public class GetParentDetailsService(EstudDbContext ctx) : IEstudService
{
    public async Task<OneOf<GetParentDetailsOut, EstudError>> Get(int parentId)
    {
        var institutionId = ctx.RequestUser.InstitutionId;

        var parent = await ctx.Parents.AsNoTracking()
            .Include(p => p.User)
            .FirstOrDefaultAsync(p => p.Id == parentId && p.InstitutionId == institutionId);
        if (parent == null) return ParentNotFound.I;

        var students = await GetStudents(parentId);

        return new GetParentDetailsOut
        {
            Id = parent.Id,
            Name = parent.Name,
            Email = parent.User!.Email!,
            PhoneNumber = parent.User.PhoneNumber,
            CreatedAt = parent.User.CreatedAt,
            Students = students,
        };
    }

    private async Task<List<GetParentDetailsStudentOut>> GetStudents(int parentId)
    {
        var students = await ctx.ParentStudents.AsNoTracking()
            .Where(x => x.ParentId == parentId)
            .OrderBy(x => x.Student!.Name)
            .Select(x => new GetParentDetailsStudentOut
            {
                Id = x.StudentId,
                Name = x.Student!.Name,
                Email = x.Student.User!.Email!,
                EnrollmentCode = x.Student.EnrollmentCode,
                Status = x.Student.Status,
                Relationship = x.Relationship,
                LinkStatus = x.Status,
                RevokedByStudent = x.RevokedByStudent,
                LinkedAt = x.CreatedAt,
            })
            .ToListAsync();

        if (students.Count == 0) return students;

        var studentIds = students.ConvertAll(s => s.Id);

        var enrollments = await ctx.StudentCourseEnrollments.AsNoTracking()
            .Where(e => studentIds.Contains(e.StudentId) && e.LeftAt == null)
            .OrderByDescending(e => e.EnrolledAt)
            .Select(e => new
            {
                e.StudentId,
                Course = e.CourseOffering!.Course!.Name,
                Campus = e.CourseOffering.Campus!.Name,
                Period = e.CourseOffering.AcademicPeriod!.Name,
            })
            .ToListAsync();

        foreach (var student in students)
        {
            var enrollment = enrollments.Find(e => e.StudentId == student.Id);
            if (enrollment == null) continue;

            student.Course = enrollment.Course;
            student.Campus = enrollment.Campus;
            student.Period = enrollment.Period;
        }

        return students;
    }
}
