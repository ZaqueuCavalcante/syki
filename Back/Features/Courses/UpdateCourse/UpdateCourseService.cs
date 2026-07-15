namespace Estud.Back.Features.Courses.UpdateCourse;

public class UpdateCourseService(EstudDbContext ctx) : IEstudService
{
    private class Validator : AbstractValidator<UpdateCourseIn>
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

    public async Task<OneOf<UpdateCourseOut, EstudError>> Update(UpdateCourseIn data)
    {
        if (V.Run(data, out var error)) return error;
        var institutionId = ctx.RequestUser.InstitutionId;

        var course = await ctx.Courses.FirstOrDefaultAsync(x => x.InstitutionId == institutionId && x.Id == data.Id);
        if (course == null) return CourseNotFound.I;

        course.Update(data.Name, data.Type!.Value);
        await ctx.SaveChangesAsync();

        return course.ToUpdateCourseOut();
    }
}
