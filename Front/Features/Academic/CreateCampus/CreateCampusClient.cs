namespace Syki.Front.Features.Academic.CreateCampus;

public class CreateCampusClient(HttpClient http) : IAcademicClient
{
    public async Task<HttpResponseMessage> Create(string name, BrazilState state, string city)
    {
        var data = new CreateCampusIn { Name = name, State = state, City = city };
        return await http.PostAsJsonAsync("/academic/campi", data);
    }
}
