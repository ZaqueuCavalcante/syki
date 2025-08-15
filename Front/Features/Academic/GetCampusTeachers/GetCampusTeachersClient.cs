namespace Syki.Front.Features.Academic.GetCampusTeachers;

public class GetCampusTeachersClient(HttpClient http) : IAcademicClient
{
    public async Task<List<GetCampusTeachersOut>> Get(Guid campusId)
    {
        return await http.GetFromJsonAsync<List<GetCampusTeachersOut>>($"/academic/campi/{campusId}/teachers", HttpConfigs.JsonOptions) ?? [];
    }
}
