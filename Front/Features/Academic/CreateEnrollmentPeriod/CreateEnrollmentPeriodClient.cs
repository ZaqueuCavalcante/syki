namespace Syki.Front.Features.Academic.CreateEnrollmentPeriod;

public class CreateEnrollmentPeriodClient(HttpClient http)
{
    public async Task<OneOf<EnrollmentPeriodOut, ErrorOut>> Create(string id, DateOnly startAt, DateOnly endAt)
    {
        var data = new CreateEnrollmentPeriodIn { Id = id, StartAt = startAt, EndAt = endAt };

        var response = await http.PostAsJsonAsync("/academic/enrollment-periods", data);

        return await response.Resolve<EnrollmentPeriodOut>();
    }
}
