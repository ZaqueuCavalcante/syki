namespace Estud.Tests.Integration.Clients;

public partial class TestsHttpClient
{
    public async Task<HttpResponseMessage> GetHealth()
    {
        return await http.GetAsync("health");
    }

    public async Task<HttpResponseMessage> GetHome()
    {
        return await http.GetAsync("");
    }

    public async Task<HttpResponseMessage> GetVersion()
    {
        return await http.GetAsync("version");
    }
}
