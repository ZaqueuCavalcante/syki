namespace Syki.Front.Features.Teacher.GetTeacherClass;

public class GetTeacherClassClient(HttpClient http) : ITeacherClient
{
    public async Task<TeacherClassOut> Get(Guid id)
    {
        return await http.GetFromJsonAsync<TeacherClassOut>($"/teacher/classes/{id}", HttpConfigs.JsonOptions) ?? new();
    }
}
