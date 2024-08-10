namespace Syki.Front.Features.Teacher.GetTeacherClasses;

public class GetTeacherClassesClient(HttpClient http) : ITeacherClient
{
    public async Task<List<TeacherClassesOut>> Get()
    {
        return await http.GetFromJsonAsync<List<TeacherClassesOut>>("/teacher/classes") ?? [];
    }
}
