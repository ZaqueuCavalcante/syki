namespace Syki.Front.Features.Academic.GetCoursesWithDisciplines;

public class GetCoursesWithDisciplinesClient(HttpClient http) : IAcademicClient
{
    public async Task<List<CreateCourseOut>> Get()
    {
        return await http.GetFromJsonAsync<List<CreateCourseOut>>("/academic/courses/with-disciplines", HttpConfigs.JsonOptions) ?? [];
    }
}
