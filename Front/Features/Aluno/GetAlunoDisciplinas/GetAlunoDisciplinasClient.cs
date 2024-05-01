namespace Syki.Front.GetAlunoDisciplinas;

public class GetAlunoDisciplinasClient(HttpClient http)
{
    public async Task<List<DisciplinaOut>> Get()
    {
        return await http.GetFromJsonAsync<List<DisciplinaOut>>("alunos/disciplinas") ?? [];
    }
}
