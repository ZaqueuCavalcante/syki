namespace Syki.Front.Features.Student.GetStudentFrequencies;

public class GetStudentFrequenciesClient(HttpClient http) : IStudentClient
{
    public async Task<OneOf<List<GetStudentFrequenciesOut>, ErrorOut>> Get()
    {
        var response = await http.GetAsync("/student/frequencies");

        return await response.Resolve<List<GetStudentFrequenciesOut>>();
    }
}
