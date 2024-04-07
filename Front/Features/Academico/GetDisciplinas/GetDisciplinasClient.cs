namespace Front.GetDisciplinas;

public class GetDisciplinasClient(HttpClient http)
{
    public async Task<List<DisciplinaOut>> Get()
    {
        return await http.GetFromJsonAsync<List<DisciplinaOut>>("/disciplinas") ?? [];
    }
}
