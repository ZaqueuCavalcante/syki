namespace Estud.Back.Features.Disciplines.GetDiscipline;

public class GetDisciplineService(EstudDbContext ctx) : IEstudService
{
    public async Task<OneOf<GetDisciplineOut, EstudError>> Get(int id)
    {
        var discipline = await ctx.Disciplines.AsNoTracking().Include(d => d.Links)
            .FirstOrDefaultAsync(d => d.InstitutionId == ctx.RequestUser.InstitutionId && d.Id == id);
        if (discipline == null) return DisciplineNotFound.I;

        var courseIds = discipline.Links.Select(l => l.CourseId).ToList();

        var courses = await ctx.Courses.AsNoTracking()
            .Where(c => courseIds.Contains(c.Id))
            .OrderBy(c => c.Name)
            .Select(c => new GetDisciplineCourseOut { Id = c.Id, Name = c.Name })
            .ToListAsync();

        return new GetDisciplineOut
        {
            Id = discipline.Id,
            Name = discipline.Name,
            Code = discipline.Code,
            Courses = courses,
        };
    }
}
