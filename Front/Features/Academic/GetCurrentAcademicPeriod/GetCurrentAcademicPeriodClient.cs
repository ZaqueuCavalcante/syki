namespace Syki.Front.Features.Academic.GetCurrentAcademicPeriod;

public class GetCurrentAcademicPeriodClient(HttpClient http) : IAcademicClient
{
    public async Task<OneOf<AcademicPeriodOut, ErrorOut>> Get()
    {
        var response = await http.GetAsync("/academic/academic-periods/current");

        return await response.Resolve<AcademicPeriodOut>();
    }
}
