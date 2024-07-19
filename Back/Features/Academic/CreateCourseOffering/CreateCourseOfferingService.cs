namespace Syki.Back.Features.Academic.CreateCourseOffering;

public class CreateCourseOfferingService(SykiDbContext ctx)
{
    public async Task<OneOf<CourseOfferingOut, SykiError>> Create(Guid institutionId, CreateCourseOfferingIn data)
    {
        var campusOk = await ctx.Campi
            .AnyAsync(c => c.InstitutionId == institutionId && c.Id == data.CampusId);
        if (!campusOk) return new CampusNotFound();

        var courseOk = await ctx.Courses
            .AnyAsync(c => c.InstitutionId == institutionId && c.Id == data.CourseId);
        if (!courseOk) return new CourseNotFound();

        var courseCurriculumOk = await ctx.CourseCurriculums
            .AnyAsync(g => g.InstitutionId == institutionId && g.Id == data.CourseCurriculumId && g.CourseId == data.CourseId);
        if (!courseCurriculumOk) return new CourseCurriculumNotFound();

        var periodOk = await ctx.AcademicPeriods
            .AnyAsync(p => p.InstitutionId == institutionId && p.Id == data.Period);
        if (!periodOk) return new AcademicPeriodNotFound();

        var courseOffering = new CourseOffering(
            institutionId,
            data.CampusId,
            data.CourseId,
            data.CourseCurriculumId,
            data.Period!,
            data.Shift
        );

        ctx.CourseOfferings.Add(courseOffering);
        await ctx.SaveChangesAsync();

        courseOffering = await ctx.CourseOfferings.AsNoTracking()
            .Include(x => x.Campus)
            .Include(x => x.Course)
            .Include(x => x.CourseCurriculum)
            .FirstAsync(x => x.Id == courseOffering.Id);

        return courseOffering.ToOut();
    }
}
