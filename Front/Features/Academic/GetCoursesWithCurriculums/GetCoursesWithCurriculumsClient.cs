namespace Syki.Front.Features.Academic.GetCoursesWithCurriculums;

public class GetCoursesWithCurriculumsClient(HttpClient http) : IAcademicClient
{
    public async Task<List<CreateCourseOut>> Get()
    {
        return await http.GetFromJsonAsync<List<CreateCourseOut>>("/academic/courses/with-curriculums", HttpConfigs.JsonOptions) ?? [];
    }
}
