namespace Syki.Front.Features.Academic.CreateAcademicPeriod;

public class CreateAcademicPeriodClient(HttpClient http)
{
    public async Task<HttpResponseMessage> Create(string id, DateOnly startAt, DateOnly endAt)
    {
        var data = new CreateAcademicPeriodIn { Id = id, StartAt = startAt, EndAt = endAt };
        return await http.PostAsJsonAsync("/academic/academic-periods", data);
    }
}
