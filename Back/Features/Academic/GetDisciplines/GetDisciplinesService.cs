namespace Syki.Back.Features.Academic.GetDisciplines;

public class GetDisciplinesService(SykiDbContext ctx) : IAcademicService
{
    public async Task<List<DisciplineOut>> Get(Guid institutionId, Guid? courseId)
    {
        var ids = await ctx.CoursesDisciplines
            .Where(cd => cd.CourseId == courseId)
            .Select(cd => cd.DisciplineId)
            .ToListAsync();

        if (courseId != null && ids.Count == 0) return [];

        var disciplines = await ctx.Disciplines
            .Where(d => d.InstitutionId == institutionId && (ids.Count == 0 || ids.Contains(d.Id)))
            .OrderBy(d => d.Name)
            .ToListAsync();

        return disciplines.ConvertAll(d => d.ToOut());
    }
}
