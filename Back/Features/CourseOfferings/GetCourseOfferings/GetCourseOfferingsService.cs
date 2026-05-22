namespace Syki.Back.Features.CourseOfferings.GetCourseOfferings;

public class GetCourseOfferingsService(SykiDbContext ctx) : ISykiService
{
    public async Task<GetCourseOfferingsOut> Get()
    {
        var offerings = await ctx.CourseOfferings.AsNoTracking()
            .Include(o => o.Campus)
            .Include(o => o.Course)
            .Include(o => o.CourseCurriculum)
            .Include(o => o.AcademicPeriod)
            .Where(o => o.InstitutionId == ctx.RequestUser.InstitutionId)
            .OrderBy(o => o.Id)
            .ToListAsync();

        return new GetCourseOfferingsOut
        {
            Total = offerings.Count,
            Items = offerings.ConvertAll(o => o.ToGetCourseOfferingsItemOut()),
        };
    }
}
