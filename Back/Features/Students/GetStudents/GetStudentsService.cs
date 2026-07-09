namespace Estud.Back.Features.Students.GetStudents;

public class GetStudentsService(EstudDbContext ctx) : IEstudService
{
    public async Task<GetStudentsOut> Get()
    {
        var institutionId = ctx.RequestUser.InstitutionId;

        var students = await ctx.Students.AsNoTracking()
            .Include(s => s.User)
            .Where(s => s.InstitutionId == institutionId)
            .OrderBy(s => s.Name)
            .ToListAsync();
        var studentIds = students.Select(s => s.Id).ToHashSet();

        var studentEnrollments = await ctx.StudentCourseEnrollments.AsNoTracking()
            .Where(e => studentIds.Contains(e.StudentId) && e.LeftAt == null)
            .ToListAsync();
        var courseOfferingIds = studentEnrollments.Select(e => e.CourseOfferingId).ToHashSet();

        var courseOfferings = await ctx.CourseOfferings.AsNoTracking()
            .Where(co => courseOfferingIds.Contains(co.Id))
            .Select(g => new { g.Id, g.CourseId })
            .ToListAsync();
        var coursesIds = courseOfferings.Select(co => co.CourseId).ToHashSet();
        var courses = await ctx.Courses.AsNoTracking()
            .Where(c => coursesIds.Contains(c.Id))
            .Select(c => new { c.Id, c.Name })
            .ToListAsync();


        var result = students.ConvertAll(s => s.ToGetStudentsItemOut());
        foreach (var item in result)
        {
            var enrollment = studentEnrollments.FirstOrDefault(e => e.StudentId == item.Id);
            if (enrollment == null) continue;

            var courseOffering = courseOfferings.FirstOrDefault(co => co.Id == enrollment.CourseOfferingId);
            if (courseOffering == null) continue;

            var course = courses.FirstOrDefault(c => c.Id == courseOffering.CourseId);
            item.Course = course?.Name ?? "-";
        }

        return new GetStudentsOut { Total = result.Count, Items = result };
    }
}
