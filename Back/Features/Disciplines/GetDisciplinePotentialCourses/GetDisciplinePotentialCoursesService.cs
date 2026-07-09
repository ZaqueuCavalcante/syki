namespace Estud.Back.Features.Disciplines.GetDisciplinePotentialCourses;

public class GetDisciplinePotentialCoursesService(EstudDbContext ctx) : IEstudService
{
    public async Task<OneOf<GetDisciplinePotentialCoursesOut, EstudError>> Get(int id, string? name)
    {
        var discipline = await ctx.Disciplines.AsNoTracking().Include(d => d.Links)
            .FirstOrDefaultAsync(d => d.InstitutionId == ctx.RequestUser.InstitutionId && d.Id == id);
        if (discipline == null) return DisciplineNotFound.I;

        var linkedCourseIds = discipline.Links.Select(l => l.CourseId).ToHashSet();

        var query = ctx.Courses.AsNoTracking()
            .Where(c => c.InstitutionId == ctx.RequestUser.InstitutionId && !linkedCourseIds.Contains(c.Id));

        if (name.HasValue()) query = query.Where(c => c.Name.ToLower().Contains(name.ToLower()));

        var items = await query
            .OrderBy(c => c.Name)
            .Select(c => new GetDisciplinePotentialCourseItemOut { Id = c.Id, Name = c.Name })
            .ToListAsync();

        return new GetDisciplinePotentialCoursesOut { Items = items };
    }
}
