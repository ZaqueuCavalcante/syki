namespace Syki.Front.Features.Student.GetCurrentEnrollmentPeriod;

public class GetCurrentEnrollmentPeriodClient(HttpClient http)
{
    public async Task<EnrollmentPeriodOut> Get()
    {
        return await http.GetFromJsonAsync<EnrollmentPeriodOut>("/student/enrollment-periods/current") ?? new();
    }
}
