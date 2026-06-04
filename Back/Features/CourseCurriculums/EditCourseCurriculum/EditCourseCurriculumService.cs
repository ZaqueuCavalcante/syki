using Syki.Back.Domain.CourseCurriculums;

namespace Syki.Back.Features.CourseCurriculums.EditCourseCurriculum;

public class EditCourseCurriculumService(SykiDbContext ctx) : ISykiService
{
    private class Validator : AbstractValidator<EditCourseCurriculumIn>
    {
        public Validator()
        {
            RuleFor(x => x.Name).NotEmpty().WithError(InvalidCourseCurriculumName.I);
            RuleFor(x => x.Name).MaximumLength(50).WithError(InvalidCourseCurriculumName.I);
        }
    }
    private static readonly Validator V = new();

    public async Task<OneOf<SykiSuccess, SykiError>> Edit(EditCourseCurriculumIn data)
    {
        if (V.Run(data, out var error)) return error;

        var curriculum = await ctx.CourseCurriculums
            .Include(c => c.Links)
            .FirstOrDefaultAsync(c => c.InstitutionId == ctx.RequestUser.InstitutionId && c.Id == data.Id);
        if (curriculum == null) return CourseCurriculumNotFound.I;

        var courseDisciplines = await ctx.CoursesDisciplines.AsNoTracking()
            .Where(x => x.CourseId == curriculum.CourseId)
            .Select(x => x.DisciplineId)
            .ToListAsync();

        if (!data.Disciplines.ConvertAll(d => d.Id).IsSubsetOf(courseDisciplines))
            return new InvalidDisciplinesList();

        curriculum.Name = data.Name;
        ctx.RemoveRange(curriculum.Links.ToList());
        curriculum.Links.Clear();
        var newLinks = data.Disciplines.ConvertAll(d => new CourseCurriculumDiscipline(d.Id, d.Period, d.Credits, d.Workload));
        curriculum.AddDisciplines(newLinks);

        await ctx.SaveChangesAsync();

        return SykiSuccess.I;
    }
}
