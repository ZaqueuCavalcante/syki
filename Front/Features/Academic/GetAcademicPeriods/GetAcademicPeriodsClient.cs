namespace Syki.Front.Features.Academic.GetAcademicPeriods;

public class GetAcademicPeriodsClient(HttpClient http)
{
    public async Task<List<AcademicPeriodOut>> Get()
    {
        return await http.GetFromJsonAsync<List<AcademicPeriodOut>>("/academic/academic-periods") ?? [];
    }
}
