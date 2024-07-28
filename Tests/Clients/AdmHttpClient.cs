using Syki.Front.Features.Adm.GetUsers;
using Syki.Front.Features.Adm.GetAdmInsights;
using Syki.Front.Features.Adm.GetInstitutions;

namespace Syki.Tests.Clients;

public class AdmHttpClient(HttpClient http)
{
    public readonly HttpClient Cross = http;

    public async Task<List<UserOut>> GetUsers()
    {
        var client = new GetUsersClient(Cross);
        return await client.Get();
    }

    public async Task<List<InstitutionOut>> GetInstitutions()
    {
        var client = new GetInstitutionsClient(Cross);
        return await client.Get();
    }

    public async Task<AdmInsightsOut> GetAdmInsights()
    {
        var client = new GetAdmInsightsClient(Cross);
        return await client.Get();
    }
}
