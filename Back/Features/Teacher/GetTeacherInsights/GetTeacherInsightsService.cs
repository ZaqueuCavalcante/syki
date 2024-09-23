namespace Syki.Back.Features.Teacher.GetTeacherInsights;

public class GetTeacherInsightsService(SykiDbContext ctx) : ITeacherService
{
    public async Task<TeacherInsightsOut> Get(Guid institutionId, Guid userId)
    {
        var classes = await ctx.Classes
            .Include(c => c.Students)
            .Include(c => c.Lessons)
            .Where(x => x.InstitutionId == institutionId && x.TeacherId == userId)
            .ToListAsync();
        
        var students = new Dictionary<Guid, Guid>();
        var lessons = 0;
        classes.ForEach(c =>
        {
            c.Students.ForEach(s => students.TryAdd(s.Id, s.Id));
            lessons += c.Lessons.Count;
        });

        // Aulas concluidas
        // Aulas retantes
        // Frequencia media dos alunos nas suas turmas

        return new() { Classes = classes.Count, Students = students.Count, Lessons = lessons };
    }
}
