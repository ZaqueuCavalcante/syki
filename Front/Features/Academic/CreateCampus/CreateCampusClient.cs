namespace Syki.Front.Features.Academic.CreateCampus;

public class CreateCampusClient(HttpClient http) : IAcademicClient
{
    public async Task<OneOf<CampusOut, ErrorOut>> Create(string name, BrazilState? state, string city, int capacity)
    {
        var data = new CreateCampusIn { Name = name, State = state, City = city, Capacity = capacity };

        var response = await http.PostAsJsonAsync("/academic/campi", data);

        return await response.Resolve<CampusOut>();
    }
}
