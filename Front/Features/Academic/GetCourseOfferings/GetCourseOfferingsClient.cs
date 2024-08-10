namespace Syki.Front.Features.Academic.GetCourseOfferings;

public class GetCourseOfferingsClient(HttpClient http) : IAcademicClient
{
    public async Task<List<CourseOfferingOut>> Get()
    {
        return await http.GetFromJsonAsync<List<CourseOfferingOut>>("/academic/course-offerings") ?? [];
    }
}
