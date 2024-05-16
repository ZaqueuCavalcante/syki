namespace Syki.Front.Features.Academic.CreateCourseOffering;

public class CreateCourseOfferingClient(HttpClient http)
{
    public async Task<HttpResponseMessage> Create(CreateCourseOfferingIn data)
    {
        return await http.PostAsJsonAsync("/academic/course-offerings", data);
    }
}
