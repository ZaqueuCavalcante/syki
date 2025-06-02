namespace Syki.Front.Features.Academic.ReprocessWebhookCall;

public class ReprocessWebhookCallClient(HttpClient http) : IAcademicClient
{
    public async Task<OneOf<SuccessOut, ErrorOut>> Reprocess(Guid id)
    {
        var response = await http.PostAsJsonAsync($"/academic/webhooks/calls/{id}/reprocess", new {});

        return await response.Resolve<SuccessOut>();
    }
}
