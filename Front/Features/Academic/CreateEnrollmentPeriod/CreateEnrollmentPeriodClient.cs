namespace Syki.Front.Features.Academic.CreateEnrollmentPeriod;

public class CreateEnrollmentPeriodClient(HttpClient http)
{
    public async Task<HttpResponseMessage> Create(string id, DateOnly start, DateOnly end)
    {
        var data = new CreateEnrollmentPeriodIn { Id = id, Start = start, End = end };
        return await http.PostAsJsonAsync("/enrollment-periods", data);
    }
}
