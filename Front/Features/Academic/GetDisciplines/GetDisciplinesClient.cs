namespace Syki.Front.Features.Academic.GetDisciplines;

public class GetDisciplinesClient(HttpClient http)
{
    public async Task<List<DisciplinaOut>> Get()
    {
        return await http.GetFromJsonAsync<List<DisciplinaOut>>("/disciplines") ?? [];
    }
}
