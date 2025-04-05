namespace Syki.Back.Features.Teacher.GetTeacherInsights;

public class GetTeacherInsightsService(SykiDbContext ctx) : ITeacherService
{
    public async Task<TeacherInsightsOut> Get(Guid institutionId, Guid userId)
    {
        var classes = await ctx.Classes.AsNoTracking()
            .Include(c => c.Students)
            .Include(c => c.Lessons)
            .Where(x => x.InstitutionId == institutionId && x.TeacherId == userId && x.Status == ClassStatus.Started)
            .ToListAsync();
        
        var students = new Dictionary<Guid, Guid>();
        var totalLessons = 0;
        var finalizedLessons = 0;
        classes.ForEach(c =>
        {
            c.Students.ForEach(s => students.TryAdd(s.Id, s.Id));
            totalLessons += c.Lessons.Count;
            finalizedLessons += c.Lessons.Count(x => x.Status == ClassLessonStatus.Finalized);
        });

        return new() { Classes = classes.Count, Students = students.Count, TotalLessons = totalLessons, FinalizedLessons = finalizedLessons };
    }
}
