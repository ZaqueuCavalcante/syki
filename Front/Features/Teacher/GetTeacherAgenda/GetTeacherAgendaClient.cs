namespace Syki.Front.Features.Teacher.GetTeacherAgenda;

public class GetTeacherAgendaClient(HttpClient http) : ITeacherClient
{
    public async Task<List<AgendaDayOut>> Get()
    {
        return await http.GetFromJsonAsync<List<AgendaDayOut>>("/teacher/agenda", HttpConfigs.JsonOptions) ?? [];
    }
}
