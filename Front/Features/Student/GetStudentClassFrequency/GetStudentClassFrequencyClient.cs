namespace Syki.Front.Features.Student.GetStudentClassFrequency;

public class GetStudentClassFrequencyClient(HttpClient http) : IStudentClient
{
    public async Task<GetStudentClassFrequencyOut> Get(Guid classId)
    {
        return await http.GetFromJsonAsync<GetStudentClassFrequencyOut>($"/student/classes/{classId}/frequency", HttpConfigs.JsonOptions) ?? new();
    }
}
