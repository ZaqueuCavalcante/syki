namespace Syki.Back.Features.Courses.GetCourse;

public class GetCourseService(SykiDbContext ctx) : ISykiService
{
    public async Task<OneOf<GetCourseOut, SykiError>> Get(int id)
    {
        var course = await ctx.Courses.AsNoTracking().Include(c => c.Links)
            .FirstOrDefaultAsync(c => c.InstitutionId == ctx.RequestUser.InstitutionId && c.Id == id);
        if (course == null) return CourseNotFound.I;

        var disciplineIds = course.Links.Select(l => l.DisciplineId).ToList();

        var disciplines = await ctx.Disciplines.AsNoTracking()
            .Where(d => disciplineIds.Contains(d.Id))
            .OrderBy(d => d.Name)
            .Select(d => new GetCourseDisciplineOut { Id = d.Id, Name = d.Name })
            .ToListAsync();

        return new GetCourseOut
        {
            Id = course.Id,
            Name = course.Name,
            Type = course.CourseType.GetDescription(),
            Disciplines = disciplines,
        };
    }
}
