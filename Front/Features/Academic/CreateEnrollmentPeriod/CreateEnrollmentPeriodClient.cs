namespace Syki.Front.Features.Academic.CreateEnrollmentPeriod;

public class CreateEnrollmentPeriodClient(HttpClient http)
{
    public async Task<HttpResponseMessage> Create(string id, DateOnly startAt, DateOnly endAt)
    {
        var data = new CreateEnrollmentPeriodIn { Id = id, StartAt = startAt, EndAt = endAt };
        return await http.PostAsJsonAsync("/academic/enrollment-periods", data);
    }
}
