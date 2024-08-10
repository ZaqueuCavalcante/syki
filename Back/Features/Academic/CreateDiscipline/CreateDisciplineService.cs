namespace Syki.Back.Features.Academic.CreateDiscipline;

public class CreateDisciplineService(SykiDbContext ctx) : IAcademicService
{
    public async Task<DisciplineOut> Create(Guid institutionId, CreateDisciplineIn data)
    {
        var discipline = new Discipline(institutionId, data.Name);

        var courses = await ctx.Courses
            .Where(c => c.InstitutionId == institutionId && data.Courses.Contains(c.Id))
            .Select(c => c.Id)
            .ToListAsync();

        courses.ForEach(id => discipline.Links.Add(new() { CourseId = id }));

        ctx.Add(discipline);
        await ctx.SaveChangesAsync();

        return discipline.ToOut();
    }
}
