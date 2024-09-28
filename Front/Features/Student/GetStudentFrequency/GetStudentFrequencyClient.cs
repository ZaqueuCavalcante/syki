namespace Syki.Front.Features.Student.GetStudentFrequency;

public class GetStudentFrequencyClient(HttpClient http) : IStudentClient
{
    public async Task<GetStudentFrequencyOut> Get()
    {
        var response = await http.GetAsync("/student/frequency");

        return await response.DeserializeTo<GetStudentFrequencyOut>();
    }
}
