namespace Syki.Front.Features.Academic.GetCampi;

public class GetCampiClient(HttpClient http) : IAcademicClient
{
    public async Task<GetCampiOut> Get()
    {
        return await http.GetFromJsonAsync<GetCampiOut>("/academic/campi", HttpConfigs.JsonOptions) ?? new();
    }
}
