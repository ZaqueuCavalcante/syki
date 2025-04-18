namespace Syki.Front.Features.Academic.GetCampi;

public class GetCampiClient(HttpClient http) : IAcademicClient
{
    public async Task<List<CampusOut>> Get()
    {
        return await http.GetFromJsonAsync<List<CampusOut>>("/academic/campi", HttpConfigs.JsonOptions) ?? [];
    }
}
