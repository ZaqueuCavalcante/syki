namespace Syki.Back.Features.Academic.CreateCourse;

public class CreateCourseService(SykiDbContext ctx, HybridCache cache) : IAcademicService
{
    internal class Validator : AbstractValidator<CreateCourseIn>
    {
        public Validator()
        {
            RuleFor(x => x.Type).NotNull().WithError(new InvalidCourseType());
            RuleFor(x => x.Type).IsInEnum().WithError(new InvalidCourseType());
        }
    }
    private static readonly Validator V = new();

    public async Task<OneOf<CourseOut, SykiError>> Create(Guid institutionId, CreateCourseIn data)
    {
        if (V.Run(data, out var error)) return error;

        var course = new Course(institutionId, data.Name, data.Type!.Value);

        data.Disciplines.ForEach(d => course.Disciplines.Add(new(institutionId, d)));

        await ctx.SaveChangesAsync(course);

        await cache.RemoveAsync($"courses:{institutionId}");

        return course.ToOut();
    }
}
