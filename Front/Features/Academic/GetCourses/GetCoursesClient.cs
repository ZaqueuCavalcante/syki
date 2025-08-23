namespace Syki.Front.Features.Academic.GetCourses;

public class GetCoursesClient(HttpClient http) : IAcademicClient
{
    public async Task<List<CreateCourseOut>> Get()
    {
        return await http.GetFromJsonAsync<List<CreateCourseOut>>("/academic/courses", HttpConfigs.JsonOptions) ?? [];
    }
}
