namespace Syki.Front.CreateAcademicPeriod;

public class CreateAcademicPeriodClient(HttpClient http)
{
    public async Task<HttpResponseMessage> Create(string id, DateOnly start, DateOnly end)
    {
        var data = new CreateAcademicPeriodIn { Id = id, Start = start, End = end };
        return await http.PostAsJsonAsync("/academic-periods", data);
    }
}
