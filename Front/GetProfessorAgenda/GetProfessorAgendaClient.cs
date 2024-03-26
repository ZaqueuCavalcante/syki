namespace Syki.Front.GetProfessorAgenda;

public class GetProfessorAgendaClient(HttpClient http)
{
    public async Task<List<AgendaDiaOut>> Get()
    {
        return await http.GetFromJsonAsync<List<AgendaDiaOut>>("/professor-agenda") ?? [];
    }
}
