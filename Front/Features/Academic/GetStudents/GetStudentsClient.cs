namespace Syki.Front.Features.Academic.GetStudents;

public class GetStudentsClient(HttpClient http) : IAcademicClient
{
    public async Task<List<StudentOut>> Get()
    {
        return await http.GetFromJsonAsync<List<StudentOut>>("/academic/students", HttpConfigs.JsonOptions) ?? [];
    }
}
