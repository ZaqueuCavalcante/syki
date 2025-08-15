namespace Syki.Front.Features.Academic.GetCampusStudents;

public class GetCampusStudentsClient(HttpClient http) : IAcademicClient
{
    public async Task<List<GetCampusStudentsOut>> Get(Guid campusId)
    {
        return await http.GetFromJsonAsync<List<GetCampusStudentsOut>>($"/academic/campi/{campusId}/students", HttpConfigs.JsonOptions) ?? [];
    }
}
