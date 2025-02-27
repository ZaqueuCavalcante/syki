namespace Syki.Front.Features.Academic.GetEnrollmentPeriods;

public class GetEnrollmentPeriodsClient(HttpClient http) : IAcademicClient
{
    public async Task<List<EnrollmentPeriodOut>> Get()
    {
        return await http.GetFromJsonAsync<List<EnrollmentPeriodOut>>("/academic/enrollment-periods", HttpConfigs.JsonOptions) ?? [];
    }
}
