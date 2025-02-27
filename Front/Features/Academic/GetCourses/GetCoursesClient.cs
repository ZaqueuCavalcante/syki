namespace Syki.Front.Features.Academic.GetCourses;

public class GetCoursesClient(HttpClient http) : IAcademicClient
{
    public async Task<List<CourseOut>> Get()
    {
        return await http.GetFromJsonAsync<List<CourseOut>>("/academic/courses", HttpConfigs.JsonOptions) ?? [];
    }
}
