namespace Syki.Front.Features.Academic.GetCampi;

public class GetCampiClient(HttpClient http) : IAcademicClient
{
    public async Task<List<CreateCampusOut>> Get()
    {
        return await http.GetFromJsonAsync<List<CreateCampusOut>>("/academic/campi", HttpConfigs.JsonOptions) ?? [];
    }
}
