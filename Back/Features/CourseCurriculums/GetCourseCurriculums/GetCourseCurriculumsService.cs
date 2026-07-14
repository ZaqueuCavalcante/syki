namespace Estud.Back.Features.CourseCurriculums.GetCourseCurriculums;

public class GetCourseCurriculumsService(EstudDbContext ctx) : IEstudService
{
    private const int MaxPageSize = 100;

    public async Task<GetCourseCurriculumsOut> Get(GetCourseCurriculumsIn query)
    {
        var page = Math.Max(query.Page, 1);
        var pageSize = Math.Clamp(query.PageSize, 1, MaxPageSize);

        var curriculumsQuery = ctx.CourseCurriculums.AsNoTracking()
            .Include(c => c.Course)
            .Where(c => c.InstitutionId == ctx.RequestUser.InstitutionId);

        var total = await curriculumsQuery.CountAsync();

        var curriculums = await curriculumsQuery
            .OrderBy(c => c.Name)
            .ThenBy(c => c.Id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        var ids = curriculums.Select(d => d.Id).ToHashSet();
        var result = curriculums.ConvertAll(d => d.ToGetCourseCurriculumsItemOut());

        if (ids.Count > 0)
        {
            var disciplines = await ctx.CourseCurriculumDisciplines.AsNoTracking()
                .Where(c => ids.Contains(c.CourseCurriculumId))
                .ToListAsync();
            result.ForEach(x => x.Disciplines = disciplines.Count(c => c.CourseCurriculumId == x.Id));
        }

        return new GetCourseCurriculumsOut
        {
            Total = total,
            Page = page,
            PageSize = pageSize,
            Items = result,
        };
    }
}
