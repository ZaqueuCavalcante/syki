namespace Syki.Front.Features.Adm.ReprocessCommand;

public class ReprocessCommandClient(HttpClient http) : IAdmClient
{
    public async Task<OneOf<SuccessOut, ErrorOut>> Reprocess(Guid id)
    {
        var response = await http.PostAsJsonAsync($"/adm/commands/{id}/reprocess", new {});

        return await response.Resolve<SuccessOut>();
    }
}
