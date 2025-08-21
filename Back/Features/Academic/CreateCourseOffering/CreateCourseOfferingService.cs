namespace Syki.Back.Features.Academic.CreateCourseOffering;

public class CreateCourseOfferingService(SykiDbContext ctx, HybridCache cache) : IAcademicService
{
    private class Validator : AbstractValidator<CreateCourseOfferingIn>
    {
        public Validator()
        {
            RuleFor(x => x.Shift).NotNull().WithError(InvalidShift.I);
            RuleFor(x => x.Shift).IsInEnum().WithError(InvalidShift.I);
        }
    }
    private static readonly Validator V = new();

    public async Task<OneOf<CourseOfferingOut, SykiError>> Create(CreateCourseOfferingIn data)
    {
        if (V.Run(data, out var error)) return error;

        if (await ctx.CampusNotFound(data.CampusId)) return CampusNotFound.I;
        if (await ctx.CourseNotFound(data.CourseId)) return CourseNotFound.I;
        if (await ctx.CourseCurriculumNotFound(data.CourseCurriculumId, data.CourseId)) return CourseCurriculumNotFound.I;
        if (await ctx.AcademicPeriodNotFound(data.Period)) return AcademicPeriodNotFound.I;

        var courseOffering = new CourseOffering(
            ctx.InstitutionId,
            data.CampusId,
            data.CourseId,
            data.CourseCurriculumId,
            data.Period!,
            data.Shift!.Value
        );

        await ctx.SaveChangesAsync(courseOffering);

        await cache.RemoveAsync($"courseOfferings:{ctx.InstitutionId}");

        courseOffering = await ctx.CourseOfferings.AsNoTracking()
            .Include(x => x.Campus)
            .Include(x => x.Course)
            .Include(x => x.CourseCurriculum)
            .FirstAsync(x => x.Id == courseOffering.Id);

        return courseOffering.ToOut();
    }
}
