namespace Syki.Front.Features.Academic.GetCourses;

public class GetCoursesClient(HttpClient http)
{
    public async Task<List<CourseOut>> Get()
    {
        return await http.GetFromJsonAsync<List<CourseOut>>("/courses") ?? [];
    }
}
