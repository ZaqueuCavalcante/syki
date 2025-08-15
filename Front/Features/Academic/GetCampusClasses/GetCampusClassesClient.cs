namespace Syki.Front.Features.Academic.GetCampusClasses;

public class GetCampusClassesClient(HttpClient http) : IAcademicClient
{
    public async Task<List<GetCampusClassesOut>> Get(Guid campusId)
    {
        return await http.GetFromJsonAsync<List<GetCampusClassesOut>>($"/academic/campi/{campusId}/classes", HttpConfigs.JsonOptions) ?? [];
    }
}
