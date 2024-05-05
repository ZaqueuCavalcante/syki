namespace Syki.Front.GetAlunoAgenda;

public class GetAlunoAgendaClient(HttpClient http)
{
    public async Task<List<AgendaDayOut>> Get()
    {
        return await http.GetFromJsonAsync<List<AgendaDayOut>>("/aluno-agenda") ?? [];
    }
}
