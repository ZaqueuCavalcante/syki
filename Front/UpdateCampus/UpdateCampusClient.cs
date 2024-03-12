namespace Front.UpdateCampus;

public class UpdateCampusClient(HttpClient http)
{
    public async Task<HttpResponseMessage> Update(Guid id, string name, string city)
    {
        var data = new UpdateCampusIn { Id = id, Name = name, City = city };
        return await http.PutAsJsonAsync("/campi", data);
    }
}
