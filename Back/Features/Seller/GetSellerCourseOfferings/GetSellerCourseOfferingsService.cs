namespace Syki.Back.Features.Seller.GetSellerCourseOfferings;

public class GetSellerCourseOfferingsService(SykiDbContext ctx)
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
