namespace Syki.Front.CreateEnrollmentPeriod;

public class GetEnrollmentPeriodsClient(HttpClient http)
{
    public async Task<List<EnrollmentPeriodOut>> Get()
    {
        return await http.GetFromJsonAsync<List<EnrollmentPeriodOut>>("/enrollment-periods") ?? [];
    }
}
