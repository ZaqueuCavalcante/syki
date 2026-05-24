using Syki.Back.Domain.Courses;

namespace Syki.Back.Features.Courses.CreateCourse;

public class CreateCourseService(SykiDbContext ctx) : ISykiService
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

        var institutionId = ctx.RequestUser.InstitutionId;
        var course = new Course(institutionId, data.Name, data.Type!.Value);

        await ctx.SaveChangesAsync(course);

        return new CreateCourseOut { Id = course.Id };
    }
}
