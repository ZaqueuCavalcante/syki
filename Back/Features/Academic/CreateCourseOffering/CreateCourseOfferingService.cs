namespace Syki.Back.Features.Academic.CreateCourseOffering;

public class CreateCourseOfferingService(SykiDbContext ctx)
{
    public async Task<CourseOfferingOut> Create(Guid institutionId, CreateCourseOfferingIn data)
    {
        var campusOk = await ctx.Campi
            .AnyAsync(c => c.InstitutionId == institutionId && c.Id == data.CampusId);
        if (!campusOk)
            Throw.DE010.Now();

        var courseOk = await ctx.Courses
            .AnyAsync(c => c.InstitutionId == institutionId && c.Id == data.CourseId);
        if (!courseOk)
            Throw.DE002.Now();

        var courseCurriculumOk = await ctx.CourseCurriculums
            .AnyAsync(g => g.InstitutionId == institutionId && g.Id == data.CourseCurriculumId && g.CourseId == data.CourseId);
        if (!courseCurriculumOk)
            Throw.DE011.Now();

        var periodOk = await ctx.AcademicPeriods
            .AnyAsync(p => p.InstitutionId == institutionId && p.Id == data.Period);
        if (!periodOk)
            Throw.DE005.Now();

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
