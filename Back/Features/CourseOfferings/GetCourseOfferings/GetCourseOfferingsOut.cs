using Syki.Back.Domain.Enums;

namespace Syki.Back.Features.CourseOfferings.GetCourseOfferings;

public class GetCourseOfferingsOut : IApiDto<GetCourseOfferingsOut>
{
    public int Total { get; set; }
    public List<GetCourseOfferingsItemOut> Items { get; set; } = [];

    public static IEnumerable<(string, GetCourseOfferingsOut)> GetExamples() =>
    [
        ("Exemplo", new() { Total = 1, Items = [new() { Id = 1, Campus = "Agreste I", Course = "ADS", CourseCurriculum = "Grade ADS 2024", Period = "2024.1", Session = CourseSession.Evening }] }),
    ];
}

public class GetCourseOfferingsItemOut
{
    public int Id { get; set; }
    public string Campus { get; set; }
    public string Course { get; set; }
    public string CourseCurriculum { get; set; }
    public string Period { get; set; }
    public CourseSession Session { get; set; }
}
