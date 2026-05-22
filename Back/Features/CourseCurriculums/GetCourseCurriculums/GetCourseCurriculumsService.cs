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

        return new GetCourseCurriculumsOut
        {
            Total = curriculums.Count,
            Items = curriculums.ConvertAll(c => c.ToGetCourseCurriculumsItemOut()),
        };
    }
}
