namespace Estud.Back.Features.Courses.GetCoursePotentialDisciplines;

public class GetCoursePotentialDisciplinesService(EstudDbContext ctx) : IEstudService
{
    public async Task<OneOf<GetCoursePotentialDisciplinesOut, EstudError>> Get(int id, string? name)
    {
        var institutionId = ctx.RequestUser.InstitutionId;

        var course = await ctx.Courses.AsNoTracking().Include(c => c.Links)
            .FirstOrDefaultAsync(c => c.InstitutionId == institutionId && c.Id == id);
        if (course == null) return CourseNotFound.I;

        var linkedDisciplineIds = course.Links.Select(l => l.DisciplineId).ToHashSet();

        var query = ctx.Disciplines.AsNoTracking()
            .Where(d => d.InstitutionId == institutionId && !linkedDisciplineIds.Contains(d.Id));

        if (name.HasValue()) query = query.Where(d => d.Name.ToLower().Contains(name.ToLower()));

        var items = await query
            .OrderBy(d => d.Name)
            .Select(d => new GetCoursePotentialDisciplineItemOut { Id = d.Id, Name = d.Name })
            .ToListAsync();

        return new GetCoursePotentialDisciplinesOut { Items = items };
    }
}
