namespace Estud.Back.Features.CourseOfferings.GetCourseOfferings;

public class GetCourseOfferingsIn : IApiDto<GetCourseOfferingsIn>
{
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;

    public static IEnumerable<(string, GetCourseOfferingsIn)> GetExamples() =>
    [
        ("Exemplo", new() { }),
    ];
}
