namespace Syki.Back.Features.CourseCurriculums.GetCourseCurriculums;

public class GetCourseCurriculumsService(SykiDbContext ctx) : ISykiService
{
    public async Task<GetCourseCurriculumsOut> Get()
    {
        var curriculums = await ctx.CourseCurriculums.AsNoTracking()
            .Include(c => c.Course)
            .Where(c => c.InstitutionId == ctx.RequestUser.InstitutionId)
            .OrderBy(c => c.Name)
            .ToListAsync();

        var ids = curriculums.Select(d => d.Id).ToHashSet();
        var result = curriculums.ConvertAll(d => d.ToGetCourseCurriculumsItemOut());

        var disciplines = await ctx.CourseCurriculumDisciplines.AsNoTracking()
            .Where(c => ids.Count == 0 || ids.Contains(c.CourseCurriculumId))
            .ToListAsync();
        result.ForEach(x => x.Disciplines = disciplines.Count(c => c.CourseCurriculumId == x.Id));

        return new GetCourseCurriculumsOut { Total = result.Count, Items = result };
    }
}
