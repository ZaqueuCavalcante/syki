namespace Syki.Front.Features.Academic.UpdateCampus;

public class UpdateCampusClient(HttpClient http) : IAcademicClient
{
    public async Task<OneOf<CreateCampusOut, ErrorOut>> Update(Guid id, string name, BrazilState? state, string city, int capacity)
    {
        var data = new UpdateCampusIn { Id = id, Name = name, State = state, City = city, Capacity = capacity };

        var response = await http.PutAsJsonAsync("/academic/campi", data);

        return await response.Resolve<CreateCampusOut>();
    }
}
