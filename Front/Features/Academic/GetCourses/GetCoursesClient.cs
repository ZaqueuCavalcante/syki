namespace Syki.Front.Features.Academic.GetCourses;

public class GetCoursesClient(HttpClient http) : IAcademicClient
{
    public async Task<GetCoursesOut> Get()
    {
        return await http.GetFromJsonAsync<GetCoursesOut>("/academic/courses", HttpConfigs.JsonOptions) ?? new();
    }
}
