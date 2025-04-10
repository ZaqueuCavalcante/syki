namespace Syki.Front.Features.Student.GetStudentCurrentClasses;

public class GetStudentCurrentClassesClient(HttpClient http) : IStudentClient
{
    public async Task<List<StudentCurrentClassOut>> Get()
    {
        return await http.GetFromJsonAsync<List<StudentCurrentClassOut>>("/student/classes/current", HttpConfigs.JsonOptions) ?? [];
    }
}
