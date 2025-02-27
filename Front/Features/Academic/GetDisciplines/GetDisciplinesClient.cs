namespace Syki.Front.Features.Academic.GetDisciplines;

public class GetDisciplinesClient(HttpClient http) : IAcademicClient
{
    public async Task<List<DisciplineOut>> Get()
    {
        return await http.GetFromJsonAsync<List<DisciplineOut>>("/academic/disciplines", HttpConfigs.JsonOptions) ?? [];
    }
}
