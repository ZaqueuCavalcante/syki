using Estud.Back.Domain.Courses;

namespace Estud.Back.Features.Courses.AddCourseDisciplines;

public class AddCourseDisciplinesService(EstudDbContext ctx) : IEstudService
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

    public async Task<OneOf<EstudSuccess, EstudError>> Add(AddCourseDisciplinesIn data)
    {
        if (V.Run(data, out var error)) return error;
        var institutionId = ctx.RequestUser.InstitutionId;

        var course = await ctx.Courses.Include(c => c.Links)
            .FirstOrDefaultAsync(x => x.InstitutionId == institutionId && x.Id == data.CourseId);
        if (course == null) return CourseNotFound.I;

        var validDisciplineIds = await ctx.Disciplines
            .Where(d => d.InstitutionId == institutionId && data.Disciplines.Contains(d.Id))
            .Select(d => d.Id)
            .ToListAsync();

        if (validDisciplineIds.Count != data.Disciplines.Count) return InvalidDisciplinesList.I;

        var existingDisciplineIds = course.Links.Select(l => l.DisciplineId).ToHashSet();
        validDisciplineIds.Where(id => !existingDisciplineIds.Contains(id)).ToList()
            .ForEach(id => course.Links.Add(new CourseDiscipline { DisciplineId = id }));

        await ctx.SaveChangesAsync();

        return EstudSuccess.I;
    }
}
