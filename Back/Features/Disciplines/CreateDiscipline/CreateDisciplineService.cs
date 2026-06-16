using Syki.Back.Domain.Disciplines;

namespace Syki.Back.Features.Disciplines.CreateDiscipline;

public class CreateDisciplineService(SykiDbContext ctx) : ISykiService
{
    private class Validator : AbstractValidator<CreateDisciplineIn>
    {
        public Validator()
        {
            RuleFor(x => x.Name).NotEmpty().WithError(InvalidDisciplineName.I);
            RuleFor(x => x.Name).MaximumLength(100).WithError(InvalidDisciplineName.I);
        }
    }
    private static readonly Validator V = new();

    public async Task<OneOf<CreateDisciplineOut, SykiError>> Create(CreateDisciplineIn data)
    {
        if (V.Run(data, out var error)) return error;

        var discipline = new Discipline(ctx.RequestUser.InstitutionId, data.Name);

        var courses = await ctx.Courses
            .Where(c => c.InstitutionId == ctx.RequestUser.InstitutionId && data.Courses.Contains(c.Id))
            .Select(c => c.Id)
            .ToListAsync();

        courses.ForEach(id => discipline.Links.Add(new() { CourseId = id }));

        ctx.Add(discipline);
        await ctx.SaveChangesAsync();

        return new CreateDisciplineOut { Id = discipline.Id };
    }
}
