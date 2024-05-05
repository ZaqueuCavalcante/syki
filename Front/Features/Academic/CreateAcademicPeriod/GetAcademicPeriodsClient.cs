namespace @Syki.Front.Features.Academic.CreateAcademicPeriod;

public class GetAcademicPeriodsClient(HttpClient http)
{
    public async Task<List<AcademicPeriodOut>> Get()
    {
        return await http.GetFromJsonAsync<List<AcademicPeriodOut>>("/academic-periods") ?? [];
    }
}
