namespace Syki.Back.Features.Academic.CreateCourseOffering;

public class CreateCourseOfferingService(SykiDbContext ctx, HybridCache cache) : IAcademicService
{
    internal class Validator : AbstractValidator<CreateCourseOfferingIn>
    {
        public Validator()
        {
            RuleFor(x => x.Shift).NotNull().WithError(new InvalidShift());
            RuleFor(x => x.Shift).IsInEnum().WithError(new InvalidShift());
        }
    }
    private static readonly Validator V = new();

    public async Task<OneOf<CourseOfferingOut, SykiError>> Create(Guid institutionId, CreateCourseOfferingIn data)
    {
        if (V.Run(data, out var error)) return error;

        var campusOk = await ctx.Campi.AnyAsync(c => c.InstitutionId == institutionId && c.Id == data.CampusId);
        if (!campusOk) return new CampusNotFound();

        var courseOk = await ctx.Courses.AnyAsync(c => c.InstitutionId == institutionId && c.Id == data.CourseId);
        if (!courseOk) return new CourseNotFound();

        var courseCurriculumOk = await ctx.CourseCurriculums
            .AnyAsync(g => g.InstitutionId == institutionId && g.Id == data.CourseCurriculumId && g.CourseId == data.CourseId);
        if (!courseCurriculumOk) return new CourseCurriculumNotFound();

        var periodExists = await ctx.AcademicPeriodExists(institutionId, data.Period);
        if (!periodExists) return new AcademicPeriodNotFound();

        var courseOffering = new CourseOffering(
            institutionId,
            data.CampusId,
            data.CourseId,
            data.CourseCurriculumId,
            data.Period!,
            data.Shift!.Value
        );

        ctx.CourseOfferings.Add(courseOffering);
        await ctx.SaveChangesAsync();

        await cache.RemoveAsync($"courseOfferings:{institutionId}");

        courseOffering = await ctx.CourseOfferings.AsNoTracking()
            .Include(x => x.Campus)
            .Include(x => x.Course)
            .Include(x => x.CourseCurriculum)
            .FirstAsync(x => x.Id == courseOffering.Id);

        return courseOffering.ToOut();
    }
}
