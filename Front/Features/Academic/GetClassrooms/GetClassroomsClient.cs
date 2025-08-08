namespace Syki.Front.Features.Academic.GetClassrooms;

public class GetClassroomsClient(HttpClient http) : IAcademicClient
{
    public async Task<List<GetClassroomsOut>> Get()
    {
        return await http.GetFromJsonAsync<List<GetClassroomsOut>>("/academic/classrooms", HttpConfigs.JsonOptions) ?? [];
    }
}
