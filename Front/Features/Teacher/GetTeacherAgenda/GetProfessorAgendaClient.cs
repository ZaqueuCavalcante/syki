namespace Syki.Front.GetProfessorAgenda;

public class GetProfessorAgendaClient(HttpClient http)
{
    public async Task<List<AgendaDayOut>> Get()
    {
        return await http.GetFromJsonAsync<List<AgendaDayOut>>("/professor-agenda") ?? [];
    }
}
