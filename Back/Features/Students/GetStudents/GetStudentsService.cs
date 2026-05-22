namespace Syki.Back.Features.Students.GetStudents;

public class GetStudentsService(SykiDbContext ctx) : ISykiService
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

        var enrollmentCounts = await ctx.StudentCourseEnrollments.AsNoTracking()
            .Where(e => studentIds.Contains(e.StudentId) && e.LeftAt == null)
            .GroupBy(e => e.StudentId)
            .Select(g => new { StudentId = g.Key, Count = g.Count() })
            .ToListAsync();

        var result = students.ConvertAll(s => s.ToGetStudentsItemOut());
        result.ForEach(s => s.ActiveEnrollments = enrollmentCounts.FirstOrDefault(e => e.StudentId == s.Id)?.Count ?? 0);

        return new GetStudentsOut { Total = result.Count, Items = result };
    }
}
