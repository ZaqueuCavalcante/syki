namespace Syki.Back.Features.Courses.GetCoursePotentialDisciplines;

public class GetCoursePotentialDisciplinesService(SykiDbContext ctx) : ISykiService
{
    public async Task<OneOf<GetCoursePotentialDisciplinesOut, SykiError>> Get(int id, string? name)
    {
        var course = await ctx.Courses.AsNoTracking().Include(c => c.Links)
            .FirstOrDefaultAsync(c => c.InstitutionId == ctx.RequestUser.InstitutionId && c.Id == id);
        if (course == null) return CourseNotFound.I;

        var linkedDisciplineIds = course.Links.Select(l => l.DisciplineId).ToHashSet();

        var query = ctx.Disciplines.AsNoTracking()
            .Where(d => d.InstitutionId == ctx.RequestUser.InstitutionId && !linkedDisciplineIds.Contains(d.Id));

        if (name.HasValue()) query = query.Where(d => d.Name.ToLower().Contains(name.ToLower()));

        var items = await query
            .OrderBy(d => d.Name)
            .Select(d => new GetCoursePotentialDisciplineItemOut { Id = d.Id, Name = d.Name })
            .ToListAsync();

        return new GetCoursePotentialDisciplinesOut { Items = items };
    }
}
