namespace Syki.Back.Features.Academic.CreateCourse;

public class CreateCourseService(SykiDbContext ctx, HybridCache cache) : IAcademicService
{
    private class Validator : AbstractValidator<CreateCourseIn>
    {
        public Validator()
        {
            RuleFor(x => x.Name).NotEmpty().WithError(InvalidCourseName.I);
            RuleFor(x => x.Name).MaximumLength(50).WithError(InvalidCourseName.I);

            RuleFor(x => x.Type).NotNull().WithError(InvalidCourseType.I);
            RuleFor(x => x.Type).IsInEnum().WithError(InvalidCourseType.I);
        }
    }
    private static readonly Validator V = new();

    public async Task<OneOf<CreateCourseOut, SykiError>> Create(CreateCourseIn data)
    {
        if (V.Run(data, out var error)) return error;

        var course = new Course(ctx.InstitutionId, data.Name, data.Type!.Value);
        data.Disciplines?.ForEach(d => course.Disciplines.Add(new(ctx.InstitutionId, d)));

        await ctx.SaveChangesAsync(course);

        await cache.RemoveAsync($"courses:{ctx.InstitutionId}");

        return course.ToCreateCourseOut();
    }
}
