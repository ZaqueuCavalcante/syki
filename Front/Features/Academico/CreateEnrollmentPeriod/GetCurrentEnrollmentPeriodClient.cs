namespace Syki.Front.CreateEnrollmentPeriod;

public class GetCurrentEnrollmentPeriodClient(HttpClient http)
{
    public async Task<EnrollmentPeriodOut> Get()
    {
        return await http.GetFromJsonAsync<EnrollmentPeriodOut>("/enrollment-periods/current") ?? new();
    }
}
