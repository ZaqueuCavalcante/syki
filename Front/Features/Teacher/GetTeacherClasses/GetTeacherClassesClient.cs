namespace Syki.Front.Features.Teacher.GetTeacherClasses;

public class GetTeacherClassesClient(HttpClient http)
{
    public async Task<List<TeacherClassOut>> Get()
    {
        return await http.GetFromJsonAsync<List<TeacherClassOut>>("/teacher/classes") ?? [];
    }
}
