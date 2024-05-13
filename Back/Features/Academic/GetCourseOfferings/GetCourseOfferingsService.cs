namespace Syki.Back.Features.Academic.GetCourseOfferings;

public class GetCourseOfferingsService(SykiDbContext ctx)
{
    public async Task<List<CourseOfferingOut>> Get(Guid institutionId)
    {
        var ofertas = await ctx.CourseOfferings.AsNoTracking()
            .Include(x => x.Campus)
            .Include(x => x.Course)
            .Include(x => x.CourseCurriculum)
            .Where(c => c.InstitutionId == institutionId)
            .ToListAsync();

        return ofertas.ConvertAll(o => o.ToOut());
    }
}
