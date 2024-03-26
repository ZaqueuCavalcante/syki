namespace Syki.Front.GetAlunoAgenda;

public class GetAlunoAgendaClient(HttpClient http)
{
    public async Task<List<AgendaDiaOut>> Get()
    {
        return await http.GetFromJsonAsync<List<AgendaDiaOut>>("/aluno-agenda") ?? [];
    }
}
