namespace Syki.Front.Features.Adm.ReprocessTask;

public class ReprocessTaskClient(HttpClient http) : IAdmClient
{
    public async Task<OneOf<SuccessOut, ErrorOut>> Reprocess(Guid id)
    {
        var response = await http.PostAsJsonAsync($"/adm/tasks/{id}/reprocess", new {});

        return await response.Resolve<SuccessOut>();
    }
}
