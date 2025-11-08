using Exato.Shared.Features.Office.GetCommand;

namespace Exato.Front.Features.Office.GetCommand;

public class GetCommandClient(HttpClient http) : IOfficeClient
{
    public async Task<OneOf<GetCommandOut, ErrorOut>> Get(Guid id)
    {
        var response = await http.GetAsync($"office/commands/{id}");

        return await response.Resolve<GetCommandOut>();
    }
}
