namespace Syki.Back.Features.Academic.GetCourseCurriculums;

public class GetCourseCurriculumsService(SykiDbContext ctx)
{
    public async Task<List<CourseCurriculumOut>> Get(Guid institutionId)
    {
        var courseCurriculums = await ctx.CourseCurriculums.AsNoTracking()
            .Where(c => c.InstitutionId == institutionId)
            .Include(g => g.Course)
            .Include(g => g.Disciplines)
            .Include(g => g.Links)
            .OrderBy(g => g.Name)
            .ToListAsync();

        return courseCurriculums.ConvertAll(g => g.ToOut());
    }
}
