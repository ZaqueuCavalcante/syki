namespace Syki.Front.Features.Adm.GetSykiTasks;

public class GetSykiTasksClient(HttpClient http) : IAdmClient
{
    public async Task<List<SykiTaskTableOut>> Get(SykiTaskTableFilterIn filters)
    {
        return await http.GetFromJsonAsync<List<SykiTaskTableOut>>("/adm/tasks".AddQueryString(filters), HttpConfigs.JsonOptions) ?? [];
    }
}
