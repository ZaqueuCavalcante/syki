using Syki.Back.Domain.Courses;

namespace Syki.Back.Features.Disciplines.AddDisciplineCourses;

public class AddDisciplineCoursesService(SykiDbContext ctx) : ISykiService
{
    private class Validator : AbstractValidator<AddDisciplineCoursesIn>
    {
        public Validator()
        {
            RuleFor(x => x.Courses).NotEmpty().WithError(InvalidCoursesList.I);
            RuleFor(x => x.Courses).Must(x => x != null && x.IsAllDistinct()).WithError(InvalidCoursesList.I);
        }
    }
    private static readonly Validator V = new();

    public async Task<OneOf<SykiSuccess, SykiError>> Add(AddDisciplineCoursesIn data)
    {
        if (V.Run(data, out var error)) return error;

        var discipline = await ctx.Disciplines.Include(d => d.Links)
            .FirstOrDefaultAsync(x => x.InstitutionId == ctx.RequestUser.InstitutionId && x.Id == data.DisciplineId);
        if (discipline == null) return DisciplineNotFound.I;

        var validCourseIds = await ctx.Courses
            .Where(c => c.InstitutionId == ctx.RequestUser.InstitutionId && data.Courses.Contains(c.Id))
            .Select(c => c.Id)
            .ToListAsync();

        if (validCourseIds.Count != data.Courses.Count) return InvalidCoursesList.I;

        var existingCourseIds = discipline.Links.Select(l => l.CourseId).ToHashSet();
        validCourseIds.Where(id => !existingCourseIds.Contains(id)).ToList()
            .ForEach(id => discipline.Links.Add(new CourseDiscipline { CourseId = id }));

        await ctx.SaveChangesAsync();

        return SykiSuccess.I;
    }
}
