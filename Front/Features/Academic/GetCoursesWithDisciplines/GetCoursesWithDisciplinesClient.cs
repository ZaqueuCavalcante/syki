namespace Syki.Front.Features.Academic.GetCoursesWithDisciplines;

public class GetCoursesWithDisciplinesClient(HttpClient http) : IAcademicClient
{
    public async Task<GetCoursesWithDisciplinesOut> Get()
    {
        return await http.GetFromJsonAsync<GetCoursesWithDisciplinesOut>("/academic/courses/with-disciplines", HttpConfigs.JsonOptions) ?? new();
    }
}
