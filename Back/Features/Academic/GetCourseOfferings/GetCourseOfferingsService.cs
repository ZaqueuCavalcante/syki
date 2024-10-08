namespace Syki.Back.Features.Academic.GetCourseOfferings;

public class GetCourseOfferingsService(SykiDbContext ctx) : IAcademicService
{
    public async Task<List<CourseOfferingOut>> Get(Guid institutionId)
    {
        var courseOfferings = await ctx.CourseOfferings.AsNoTracking()
            .Include(x => x.Campus)
            .Include(x => x.Course)
            .Include(x => x.CourseCurriculum)
            .Where(c => c.InstitutionId == institutionId)
            .ToListAsync();

        return courseOfferings.ConvertAll(o => o.ToOut());
    }
}
