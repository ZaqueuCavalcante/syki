namespace Syki.Front.Features.Student.GetStudentAgenda;

public class GetStudentAgendaClient(HttpClient http)
{
    public async Task<List<AgendaDayOut>> Get()
    {
        return await http.GetFromJsonAsync<List<AgendaDayOut>>("/student-agenda") ?? [];
    }
}
