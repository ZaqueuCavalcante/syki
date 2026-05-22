using Syki.Back.Domain.Courses;

namespace Syki.Back.Features.Courses.CreateCourseCurriculum;

public class CreateCourseCurriculumService(SykiDbContext ctx) : ISykiService
{
    private class Validator : AbstractValidator<CreateCourseCurriculumIn>
    {
        public Validator()
        {
            RuleFor(x => x.Name).NotEmpty().WithError(InvalidCourseCurriculumName.I);
            RuleFor(x => x.Name).MaximumLength(50).WithError(InvalidCourseCurriculumName.I);
        }
    }
    private static readonly Validator V = new();

    public async Task<OneOf<CreateCourseCurriculumOut, SykiError>> Create(CreateCourseCurriculumIn data)
    {
        if (V.Run(data, out var error)) return error;

        var courseOk = await ctx.Courses.AnyAsync(c => c.InstitutionId == ctx.RequestUser.InstitutionId && c.Id == data.CourseId);
        if (!courseOk) return CourseNotFound.I;

        var courseDisciplines = await ctx.CoursesDisciplines.AsNoTracking()
            .Where(x => x.CourseId == data.CourseId)
            .Select(x => x.DisciplineId)
            .ToListAsync();

        if (!data.Disciplines.ConvertAll(d => d.Id).IsSubsetOf(courseDisciplines))
            return new InvalidDisciplinesList();

        var courseCurriculum = new CourseCurriculum(ctx.RequestUser.InstitutionId, data.CourseId, data.Name);
        var disciplines = data.Disciplines.ConvertAll(d => new CourseCurriculumDiscipline(d.Id, d.Period, d.Credits, d.Workload));
        courseCurriculum.AddDisciplines(disciplines);

        await ctx.SaveChangesAsync(courseCurriculum);

        return new CreateCourseCurriculumOut { Id = courseCurriculum.Id };
    }
}
