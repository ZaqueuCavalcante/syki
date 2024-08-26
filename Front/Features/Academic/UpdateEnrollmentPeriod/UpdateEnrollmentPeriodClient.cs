namespace Syki.Front.Features.Academic.UpdateEnrollmentPeriod;

public class UpdateEnrollmentPeriodClient(HttpClient http) : IAcademicClient
{
    public async Task<OneOf<EnrollmentPeriodOut, ErrorOut>> Update(string id, DateOnly startAt, DateOnly endAt)
    {
        var data = new UpdateEnrollmentPeriodIn { StartAt = startAt, EndAt = endAt };

        var response = await http.PutAsJsonAsync($"/academic/enrollment-periods/{id}", data);

        return await response.Resolve<EnrollmentPeriodOut>();
    }
}
