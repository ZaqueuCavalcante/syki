using Syki.Front.Features.Adm.GetUsers;
using Syki.Front.Features.Adm.GetInstitutions;
using Syki.Front.Features.Adm.GetAdmInsights;

namespace Syki.Tests.Clients;

public static class AdmClientExtensions
{
    public static async Task<List<UserOut>> GetUsers(this HttpClient http)
    {
        var client = new GetUsersClient(http);
        return await client.Get();
    }

    public static async Task<List<InstitutionOut>> GetInstitutions(this HttpClient http)
    {
        var client = new GetInstitutionsClient(http);
        return await client.Get();
    }

    public static async Task<AdmInsightsOut> GetAdmInsights(this HttpClient http)
    {
        var client = new GetAdmInsightsClient(http);
        return await client.Get();
    }
}
