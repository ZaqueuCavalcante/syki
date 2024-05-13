namespace Syki.Back.Features.Academic.GetCourseCurriculumDisciplines;

public class GetCourseCurriculumDisciplinesService(SykiDbContext ctx)
{
    public async Task<List<DisciplineOut>> Get(Guid institutionId, Guid id)
    {
        var courseCurriculum = await ctx.CourseCurriculums.AsNoTracking()
            .Where(g => g.InstitutionId == institutionId && g.Id == id)
            .Include(g => g.Disciplines)
            .FirstOrDefaultAsync();
        
        if (courseCurriculum == null) return [];

        return courseCurriculum.Disciplines.ConvertAll(d => d.ToOut()).OrderBy(d => d.Name).ToList();
    }
}
