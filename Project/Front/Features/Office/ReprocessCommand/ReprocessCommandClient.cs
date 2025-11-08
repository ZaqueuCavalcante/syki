namespace Exato.Front.Features.Office.ReprocessCommand;

public class ReprocessCommandClient(HttpClient http) : IOfficeClient
{
    public async Task<OneOf<SuccessOut, ErrorOut>> Reprocess(Guid id)
    {
        var response = await http.PostAsJsonAsync($"office/commands/{id}/reprocess", new {});

        return await response.Resolve<SuccessOut>();
    }
}
