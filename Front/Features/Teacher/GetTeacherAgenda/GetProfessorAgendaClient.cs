namespace Syki.Front.Features.Teacher.GetTeacherAgenda;

public class GetProfessorAgendaClient(HttpClient http)
{
    public async Task<List<AgendaDayOut>> Get()
    {
        return await http.GetFromJsonAsync<List<AgendaDayOut>>("/professor-agenda") ?? [];
    }
}
