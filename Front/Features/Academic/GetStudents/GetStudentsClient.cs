namespace Syki.Front.Features.Academic.GetStudents;

public class GetStudentsClient(HttpClient http)
{
    public async Task<List<StudentOut>> Get()
    {
        return await http.GetFromJsonAsync<List<StudentOut>>("/academic/students") ?? [];
    }
}
