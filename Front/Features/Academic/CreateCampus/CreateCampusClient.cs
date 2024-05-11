namespace Syki.Front.Features.Academic.CreateCampus;

public class CreateCampusClient(HttpClient http)
{
    public async Task<HttpResponseMessage> Create(string name, string city)
    {
        var data = new CreateCampusIn { Name = name, City = city };
        return await http.PostAsJsonAsync("/academic/campi", data);
    }
}
