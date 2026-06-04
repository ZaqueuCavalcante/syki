using Syki.Back.Domain.Courses;

namespace Syki.Back.Features.Courses.AddCourseDisciplines;

public class AddCourseDisciplinesService(SykiDbContext ctx) : ISykiService
{
    private class Validator : AbstractValidator<AddCourseDisciplinesIn>
    {
        public Validator()
        {
            RuleFor(x => x.Disciplines).NotEmpty().WithError(InvalidDisciplinesList.I);
            RuleFor(x => x.Disciplines).Must(x => x != null && x.IsAllDistinct()).WithError(InvalidDisciplinesList.I);
        }
    }
    private static readonly Validator V = new();

    public async Task<OneOf<SykiSuccess, SykiError>> Add(AddCourseDisciplinesIn data)
    {
        if (V.Run(data, out var error)) return error;

        var course = await ctx.Courses.Include(c => c.Links)
            .FirstOrDefaultAsync(x => x.InstitutionId == ctx.RequestUser.InstitutionId && x.Id == data.CourseId);
        if (course == null) return CourseNotFound.I;

        var validDisciplineIds = await ctx.Disciplines
            .Where(d => d.InstitutionId == ctx.RequestUser.InstitutionId && data.Disciplines.Contains(d.Id))
            .Select(d => d.Id)
            .ToListAsync();

        if (validDisciplineIds.Count != data.Disciplines.Count) return InvalidDisciplinesList.I;

        var existingDisciplineIds = course.Links.Select(l => l.DisciplineId).ToHashSet();
        validDisciplineIds.Where(id => !existingDisciplineIds.Contains(id)).ToList()
            .ForEach(id => course.Links.Add(new CourseDiscipline { DisciplineId = id }));

        await ctx.SaveChangesAsync();

        return SykiSuccess.I;
    }
}
