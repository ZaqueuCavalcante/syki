namespace Syki.Front.Features.Academic.UpdateCampus;

public class UpdateCampusClient(HttpClient http)
{
    public async Task<OneOf<CampusOut, ErrorOut>> Update(Guid id, string name, string city)
    {
        var data = new UpdateCampusIn { Id = id, Name = name, City = city };

        var response = await http.PutAsJsonAsync("/academic/campi", data);

        return await response.Resolve<CampusOut>();
    }
}
