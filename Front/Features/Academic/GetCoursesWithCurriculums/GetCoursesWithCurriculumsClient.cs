namespace Syki.Front.Features.Academic.GetCoursesWithCurriculums;

public class GetCoursesWithCurriculumsClient(HttpClient http) : IAcademicClient
{
    public async Task<GetCoursesWithCurriculumsOut> Get()
    {
        return await http.GetFromJsonAsync<GetCoursesWithCurriculumsOut>("/academic/courses/with-curriculums", HttpConfigs.JsonOptions) ?? new();
    }
}
