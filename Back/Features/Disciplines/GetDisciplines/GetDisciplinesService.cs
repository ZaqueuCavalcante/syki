namespace Syki.Back.Features.Disciplines.GetDisciplines;

public class GetDisciplinesService(SykiDbContext ctx) : ISykiService
{
    public async Task<GetDisciplinesOut> Get()
    {
        var disciplines = await ctx.Disciplines.AsNoTracking()
            .Where(d => d.InstitutionId == ctx.RequestUser.InstitutionId)
            .OrderBy(d => d.Name)
            .ToListAsync();

        var ids = disciplines.Select(d => d.Id).ToHashSet();

        var result = disciplines.ConvertAll(d => d.ToGetDisciplinesItemOut());
    
        var courses = await ctx.CoursesDisciplines.AsNoTracking()
            .Where(c => ids.Count == 0 || ids.Contains(c.DisciplineId))
            .ToListAsync();
        result.ForEach(x => x.Courses = courses.Count(c => c.DisciplineId == x.Id));

        var teachers = await ctx.TeachersDisciplines.AsNoTracking()
            .Where(d => ids.Count == 0 || ids.Contains(d.DisciplineId))
            .ToListAsync();
        result.ForEach(x => x.Teachers = teachers.Count(t => t.DisciplineId == x.Id));

        return new GetDisciplinesOut { Total = result.Count, Items = result };
    }
}
