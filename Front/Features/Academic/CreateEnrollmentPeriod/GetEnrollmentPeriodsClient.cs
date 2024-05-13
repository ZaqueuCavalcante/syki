namespace Syki.Front.Features.Academic.CreateEnrollmentPeriod;

public class GetEnrollmentPeriodsClient(HttpClient http)
{
    public async Task<List<EnrollmentPeriodOut>> Get()
    {
        return await http.GetFromJsonAsync<List<EnrollmentPeriodOut>>("/academic/enrollment-periods") ?? [];
    }
}
