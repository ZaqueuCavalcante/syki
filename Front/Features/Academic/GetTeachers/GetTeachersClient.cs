namespace Syki.Front.Features.Academic.GetTeachers;

public class GetTeachersClient(HttpClient http) : IAcademicClient
{
    public async Task<List<TeacherOut>> Get()
    {
        return await http.GetFromJsonAsync<List<TeacherOut>>("/academic/teachers", HttpConfigs.JsonOptions) ?? [];
    }
}
