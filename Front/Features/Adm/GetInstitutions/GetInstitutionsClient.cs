namespace Syki.Front.Features.Adm.GetInstitutions;

public class GetInstitutionsClient(HttpClient http) : IAdmClient
{
    public async Task<List<InstitutionOut>> Get()
    {
        return await http.GetFromJsonAsync<List<InstitutionOut>>("/adm/institutions", HttpConfigs.JsonOptions) ?? [];
    }
}
