namespace Syki.Front.Features.Teacher.GetClassroomAgenda;

public class GetClassroomAgendaClient(HttpClient http) : ITeacherClient
{
    public async Task<List<AgendaDayOut>> Get(Guid classroomId)
    {
        return await http.GetFromJsonAsync<List<AgendaDayOut>>($"/academic/classrooms/{classroomId}/agenda", HttpConfigs.JsonOptions) ?? [];
    }
}
