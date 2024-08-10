namespace Syki.Front.Features.Academic.CreateAcademicPeriod;

public class CreateAcademicPeriodClient(HttpClient http) : IAcademicClient
{
    public async Task<OneOf<AcademicPeriodOut, ErrorOut>> Create(string id, DateOnly startAt, DateOnly endAt)
    {
        var data = new CreateAcademicPeriodIn { Id = id, StartAt = startAt, EndAt = endAt };

        var response = await http.PostAsJsonAsync("/academic/academic-periods", data);

        return await response.Resolve<AcademicPeriodOut>();
    }
}
