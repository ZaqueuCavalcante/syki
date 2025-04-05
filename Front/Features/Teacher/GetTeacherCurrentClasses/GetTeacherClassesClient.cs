namespace Syki.Front.Features.Teacher.GetTeacherCurrentClasses;

public class GetTeacherCurrentClassesClient(HttpClient http) : ITeacherClient
{
    public async Task<List<TeacherCurrentClassOut>> Get()
    {
        return await http.GetFromJsonAsync<List<TeacherCurrentClassOut>>("/teacher/classes/current", HttpConfigs.JsonOptions) ?? [];
    }
}
