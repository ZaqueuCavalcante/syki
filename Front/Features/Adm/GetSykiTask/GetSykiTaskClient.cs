namespace Syki.Front.Features.Adm.GetSykiTask;

public class GetSykiTaskClient(HttpClient http) : IAdmClient
{
    public async Task<SykiTaskOut> Get(Guid id)
    {
        return await http.GetFromJsonAsync<SykiTaskOut>($"/adm/tasks/{id}", HttpConfigs.JsonOptions) ?? new();
    }
}
