namespace Estud.Back.Features.CourseOfferings.GetCourseOfferings;

public class GetCourseOfferingsService(EstudDbContext ctx) : IEstudService
{
    private const int MaxPageSize = 100;

    public async Task<GetCourseOfferingsOut> Get(GetCourseOfferingsIn query)
    {
        var page = Math.Max(query.Page, 1);
        var pageSize = Math.Clamp(query.PageSize, 1, MaxPageSize);

        var offeringsQuery = ctx.CourseOfferings.AsNoTracking()
            .Where(o => o.InstitutionId == ctx.RequestUser.InstitutionId);

        var total = await offeringsQuery.CountAsync();

        var offerings = await offeringsQuery
            .Include(o => o.Campus)
            .Include(o => o.Course)
            .Include(o => o.CourseCurriculum)
            .Include(o => o.AcademicPeriod)
            .OrderBy(o => o.Id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new GetCourseOfferingsOut
        {
            Total = total,
            Page = page,
            PageSize = pageSize,
            Items = offerings.ConvertAll(o => o.ToGetCourseOfferingsItemOut()),
        };
    }
}
