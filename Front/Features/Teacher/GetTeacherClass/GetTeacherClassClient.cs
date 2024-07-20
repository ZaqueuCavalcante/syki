namespace Syki.Front.Features.Teacher.GetTeacherClass;

public class GetTeacherClassClient(HttpClient http)
{
    public async Task<TeacherClassOut> Get(Guid id)
    {
        return await http.GetFromJsonAsync<TeacherClassOut>($"/teacher/classes/{id}") ?? new();
    }
}
