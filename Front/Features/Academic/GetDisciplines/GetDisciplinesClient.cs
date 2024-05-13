namespace Syki.Front.Features.Academic.GetDisciplines;

public class GetDisciplinesClient(HttpClient http)
{
    public async Task<List<DisciplineOut>> Get()
    {
        return await http.GetFromJsonAsync<List<DisciplineOut>>("/academic/disciplines") ?? [];
    }
}
