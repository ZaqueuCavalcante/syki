namespace Estud.Back.Features.Students.GetStudents;

public class GetStudentsService(EstudDbContext ctx) : IEstudService
{
    private const int MaxPageSize = 100;

    public async Task<GetStudentsOut> Get(GetStudentsIn query)
    {
        var page = Math.Max(query.Page, 1);
        var pageSize = Math.Clamp(query.PageSize, 1, MaxPageSize);

        var institutionId = ctx.RequestUser.InstitutionId;

        var studentsQuery = ctx.Students.AsNoTracking()
            .Include(s => s.User)
            .Where(s => s.InstitutionId == institutionId);

        var filter = query.Filter;
        if (filter.HasValue())
            studentsQuery = studentsQuery.Where(s =>
                s.Name.ToLower().Contains(filter.ToLower()) ||
                s.User!.Email!.ToLower().Contains(filter.ToLower()) ||
                s.EnrollmentCode.ToLower().Contains(filter.ToLower()));

        var total = await studentsQuery.CountAsync();

        var students = await studentsQuery
            .OrderBy(s => s.Name)
            .ThenBy(s => s.Id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
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

        return new GetStudentsOut
        {
            Total = total,
            Page = page,
            PageSize = pageSize,
            Items = result,
        };
    }
}
