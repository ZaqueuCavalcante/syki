using Exato.Shared.Features.Office.GetCommandBatch;

namespace Exato.Front.Features.Office.GetCommandBatch;

public class GetCommandBatchClient(HttpClient http) : IOfficeClient
{
    public async Task<OneOf<GetCommandBatchOut, ErrorOut>> Get(Guid id)
    {
        var response = await http.GetAsync($"office/command-batches/{id}");

        return await response.Resolve<GetCommandBatchOut>();
    }
}
