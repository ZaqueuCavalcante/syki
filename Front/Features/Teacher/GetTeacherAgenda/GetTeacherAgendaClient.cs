namespace Syki.Front.Features.Teacher.GetTeacherAgenda;

public class GetTeacherAgendaClient(HttpClient http)
{
    public async Task<List<AgendaDayOut>> Get()
    {
        return await http.GetFromJsonAsync<List<AgendaDayOut>>("/teacher/agenda") ?? [];
    }
}
