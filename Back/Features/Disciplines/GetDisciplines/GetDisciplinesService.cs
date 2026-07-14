namespace Estud.Back.Features.Disciplines.GetDisciplines;

public class GetDisciplinesService(EstudDbContext ctx) : IEstudService
{
    private const int MaxPageSize = 100;

    public async Task<GetDisciplinesOut> Get(GetDisciplinesIn query)
    {
        var page = Math.Max(query.Page, 1);
        var pageSize = Math.Clamp(query.PageSize, 1, MaxPageSize);

        var disciplinesQuery = ctx.Disciplines.AsNoTracking()
            .Where(d => d.InstitutionId == ctx.RequestUser.InstitutionId);

        var filter = query.Filter;
        if (filter.HasValue())
            disciplinesQuery = disciplinesQuery.Where(d =>
                d.Name.ToLower().Contains(filter.ToLower()) ||
                d.Code.ToLower().Contains(filter.ToLower()));

        var total = await disciplinesQuery.CountAsync();

        var disciplines = await disciplinesQuery
            .OrderBy(d => d.Name)
            .ThenBy(d => d.Id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        var ids = disciplines.Select(d => d.Id).ToHashSet();

        var result = disciplines.ConvertAll(d => d.ToGetDisciplinesItemOut());

        if (ids.Count > 0)
        {
            var courses = await ctx.CoursesDisciplines.AsNoTracking()
                .Where(c => ids.Contains(c.DisciplineId))
                .ToListAsync();
            result.ForEach(x => x.Courses = courses.Count(c => c.DisciplineId == x.Id));

            var teachers = await ctx.TeachersDisciplines.AsNoTracking()
                .Where(d => ids.Contains(d.DisciplineId))
                .ToListAsync();
            result.ForEach(x => x.Teachers = teachers.Count(t => t.DisciplineId == x.Id));
        }

        return new GetDisciplinesOut
        {
            Total = total,
            Page = page,
            PageSize = pageSize,
            Items = result,
        };
    }
}
